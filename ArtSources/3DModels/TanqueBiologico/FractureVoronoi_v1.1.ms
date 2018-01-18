-- * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
-- *  FRACTURE VORONOI - v1.1 - april 2009 - 3ds max 9     *
-- *                                                       *
-- *  Divides an object in parts (Voronoi cells).          *
-- *  - several iterations;                                *
-- *  - original UVs are preserved and 'projected' onto    *
-- *  the new faces;                                       *
-- *  - simple planar mapping is applied to a new channel  *
-- *  and consistent throughout the new parts;             *
-- *  - new material ID is applied to the new faces;       *
-- *  - can keep intermediate generations;                 *
-- *  - can build hierarchy.                               *
-- *                                                       *
-- *  GARP - 2009                                          *
-- * * * * * * * * * * * * * * * * * * * * * * * * * * * * *



(	-- start script

global rltFractureVoronoi
try destroyDialog rltFractureVoronoi catch()

rollout rltFractureVoronoi "F.R.A.C.T.U.R.E"
(
	groupBox boxSetUp "" pos:[5,0] width:116 height:83
	fn geometryFilter obj = superClassOf obj == GeometryClass
	pickButton pbObject "Pick Object" pos:[10,11] width:106 height:25 filter:geometryFilter tooltip:"object to fracture"
	spinner spnNbParts "Nb Parts: " pos:[09,41] width:106 height:16 range:[2,1000,10] type:#integer enabled:false
	spinner spnNbIter "Iterations:     " pos:[28,61] width:87 height:16 range:[1,10,1] type:#integer enabled:false

	groupBox boxMat "" pos:[5,81] width:116 height:89
	spinner spnNewID "New Mat ID:" pos:[24,93] width:91 height:16 range:[1,100,1] indeterminate:true type:#integer enabled:false
	checkBox cbKeepMat "Keep Material" pos:[12,110] checked:true enabled:false
	spinner spnNewCh "New Map Ch:" pos:[32,133] width:83 height:16 range:[1,10,3] type:#integer enabled:false
	checkBox cbRWMS "R-W Map Size" pos:[12,150] checked:true enabled:false

	groupBox boxHierarchy "" pos:[5,168] width:116 height:48
	checkBox cbKeepGen "Keep Iterations" pos:[12,179] checked:false enabled:false
	checkBox cbLinkGen "Build Hierarchy" pos:[12,196] checked:false enabled:false

	groupBox boxCreate "" pos:[5,214] width:116 height:109
	button btnCreate "Break in 10" pos:[10,225] width:106 height:25 tooltip:"pick object first" enabled:false
	radiobuttons rdoColor "" pos:[12,253] width:72 height:32 labels:#("Multi Color","Uniform") default:1 columns:1 enabled:false
	colorPicker cpParts "" pos:[70,270] fieldWidth:20 height:12 visible:false
	radiobuttons rdoCenter "" pos:[12,288] width:72 height:32 labels:#("B.Boxes Centers","Vo.Cells Centers") default:1 columns:1 enabled:false

	groupBox boxProgress "" pos:[5,321] width:116 height:49
	progressBar pbProgress "" pos:[10,335] width:106 height:15 value:0 color:[0,96,0]
	label lblProStatus "" pos:[10,351] width:100 height:17

	local theObject			-- holds the original object


	on pbObject picked obj do
	(
		pbObject.text = obj.name
		theObject = obj
		spnNbParts.enabled = true
		spnNbIter.enabled = true
		spnNewID.enabled = true
		cbKeepMat.enabled = true
		spnNewCh.enabled = true
		cbRWMS.enabled = true
		cbLinkGen.enabled = true
		btnCreate.enabled = true
		btnCreate.tooltip = "start creating parts"
		rdoColor.enabled = true
		rdoCenter.enabled = true
		cpParts.color = obj.wireColor
		cpParts.visible = true
			
		when obj deleted do
		(
			btnCreate.enabled = false
			btnCreate.tooltip = pbObject.text + " has been deleted!"
			pbObject.text = "Pick Object"
		)
		
		undo off
		(	-- gets new mat ID for new faces
			m = edit_mesh()
			addModifier obj m
			spnNewID.value = amax(for i = 1 to obj.numfaces collect getFaceMatID obj i) + 1
			deleteModifier obj m
		)

	)	-- end on btnMesh picked theMesh


	on btnCreate pressed do
	(
		undo off
		(
			disableSceneRedraw()
			clearSelection()
			start = timeStamp()
			
			local nbParts = spnNbParts.value
			local nbIter = spnNbIter.value
			local keepGen = cbKeepGen.checked
			local linkGen = cbLinkGen.checked
			local aPartsStart = #()
			local aPartsEnd = #()
			local aAllParts = #()
			local aAllCoords = #()
			local thePlane = plane width:1 length:1 widthSegs:1 lengthSegs:1	-- plane helper for slice plane
			local theMesh = editable_mesh()
			local abortBreaking = false
			
			lblProStatus.caption = " Breaking..."
			
			--	BREAKING UP
			---------------
			
			-- clean copy (no custom attributes, keyframes, weird transforms, etc
			theCopy = copy theObject
			theCopy.name = "toto"
			resetXForm theCopy
			convertToMesh theCopy
			theMesh.mesh = theCopy.mesh
			theMesh.transform = theCopy.transform
			theMesh.pivot = [0,0,0]
			resetXForm theMesh
			convertToMesh theMesh
			delete theCopy
			
			-- material and UVs
			if cbKeepMat.checked do theMesh.material = theObject.material
			addModifier theMesh (uvwMap mapChannel:spnNewCh.value realWorldMapSize:cbRWMS.checked)
			convertToMesh theMesh
			setFaceSelection theMesh #{}
			
			-- parts creation
			aPartsEnd = #(theMesh)
			for iter = 1 to nbIter while not abortBreaking do
			(
				aPartsStart = aPartsEnd
				aPartsEnd = #()
				
				for obj in aPartsStart while not abortBreaking do
				(	
					aPartsTemp = for i = 1 to nbParts collect copy obj
					pSys = pcloud emitter:obj formation:3 total_number:nbParts quantityMethod:1 viewPercent:100 seed:(random 0 100)
					aCoords = for i = 1 to nbParts collect particlePos pSys i	-- fill with random coordinates
					delete pSys
					for i = 1 to nbParts - 1 do for j = i + 1 to nbParts while not abortBreaking do	-- for each pair of coords
					(
						thePlane.pos = (aCoords[i] + aCoords[j]) / 2
						thePlane.dir = aCoords[j] - aCoords[i]
						
						addModifier aPartsTemp[i] (sliceModifier slice_type:2)
						addModifier aPartsTemp[j] (sliceModifier slice_type:3)
						aPartsTemp[i].slice.slice_plane.transform = thePlane.transform
						aPartsTemp[j].slice.slice_plane.transform = thePlane.transform
						addModifier aPartsTemp[i] (cap_holes())
						addModifier aPartsTemp[j] (cap_holes())
						convertToMesh aPartsTemp[i]
						convertToMesh aPartsTemp[j]
						
						if keyboard.escPressed do abortBreaking = queryBox "Do you want to abort and delete already created parts?"
					)	-- end i loop
					aPartsEnd += aPartsTemp
					aAllParts += aPartsTemp
					aAllCoords += aCoords
					
					total = nbParts * ((nbParts^nbIter - 1) / (nbParts - 1))
					prog = 100 * aAllParts.count / total
					pbProgress.value = prog
					pbProgress.color = [200 - prog * 2,prog * 2,0]
				)	-- end obj loop
			)	-- end iter loop
			
			if not abortBreaking then
			(
				lblProStatus.caption = " Finalizing..."
				
				-- 	TIDYING UP
				--------------
				
				delete theMesh
				delete thePlane
				hide theObject
				
				-- intermediate generations
				if not keepGen and nbIter != 1 do
				(
					ind = 0
					for i = 1 to nbIter - 1 do for j = 1 to nbParts^i do
					(
					ind += 1
					delete aAllParts[ind]
					aAllCoords[ind] = undefined
					)
					aAllParts = for obj in aAllParts where not isDeleted obj collect obj
					aAllCoords = for c in aAllCoords where c != undefined collect c
				)
				
				-- coordinates
				if rdoCenter.state == 1 then centerPivot aAllParts
				else for i = 1 to aAllParts.count do aAllParts[i].pivot = aAllCoords[i]
				resetXForm aAllParts
				convertToMesh aAllParts
				
				-- new faces ID
				newID = spnNewID.value
				for obj in aAllParts do
				(
					for f in getFaceSelection obj do setFaceMatID obj f newID
					setFaceSelection obj #{}
				)
				
				-- names
				if not keepGen or nbIter == 1 then
					for i = 1 to aAllParts.count do aAllParts[i].name = theObject.name + "_Part_" + i as string
				else
				(
					for i = 1 to nbParts do aAllParts[i].name = theObject.name + "_Part_" + i as string
					indP = 0
					indC = nbParts
					for i = 1 to nbIter - 1 do for j = 1 to nbParts^i do
					(
						indP += 1
						for k = 1 to nbParts do
						(
							indC += 1
							aAllParts[indC].name = aAllParts[indP].name + "_" + k as string
						)	-- end k loop
					)	-- end j loop
				)	-- end else
				
				-- layers
				-- (comment out this block if you don't want any layer, intermediate generations will not be hidden)
				-- (FROM HERE...)
				if not keepGen or nbIter == 1 then
				(
					if layerManager.getLayerFromName (theObject.name + "_Parts") == undefined then
						theLayer = layerManager.newLayerFromName (theObject.name + "_Parts")
					else theLayer = layerManager.getLayerFromName (theObject.name + "_Parts")
					for obj in aAllParts do theLayer.addNode obj
				)	-- end if
				else
				(
					aTheLayers = for i = 1 to nbIter collect
					(
						if layerManager.getLayerFromName (theObject.name + "_Gen_" + i as string) == undefined then
							layerManager.newLayerFromName (theObject.name + "_Gen_" + i as string)
						else layerManager.getLayerFromName (theObject.name + "_Gen_" + i as string)
					)
					for i = 1 to nbIter - 1 do aTheLayers[i].isHidden = true
					ind = 0
					for i = 1 to nbIter do for j = 1 to nbParts^i do
					(
						ind += 1
						aTheLayers[i].addNode aAllParts[ind]
					)	-- end i loop
				)	-- end else
				-- (...TO HERE)
	
				-- hierarchy
				if linkGen do
				(
					if not KeepGen or nbIter == 1 then for obj in aAllParts do attachObjects theObject obj move:false
					else
					(
						for i = 1 to nbParts do attachObjects theObject aAllParts[i] move:false
						indP = 0
						indC = nbParts
						for i = 1 to nbIter - 1 do for j = 1 to nbParts^i do
						(
							indP += 1
							for k = 1 to nbParts do
							(
								indC += 1
								attachObjects aAllParts[indP] aAllParts[indC] move:false
							)	-- end k loop
						)	-- end j loop
					)	-- end else
				)	-- end if linkGen
				
				-- colors
				if rdoColor.state == 1 then for obj in aAllParts do obj.wireColor = random black white
				else aAllParts.wireColor = cpParts.color
				
				lblProStatus.caption = " Done in " + (formattedPrint ((timeStamp() - start) / 1000.0) format:".1f") + "sec."
				
				enableSceneRedraw()
				completeRedraw()
			)
			else
			(
				delete thePlane
				delete theMesh
				delete aAllParts
				pbProgress.value = 0
				lblProStatus.caption = " Stopped"
				enableSceneRedraw()
			)	-- end test abortBreaking
			
		)	-- end undo off
		
	)	-- end btnCreate pressed


	on spnNbParts changed val do
	(
		btnCreate.caption = "Break in " + ((val ^ spnNbIter.value) as string)
	)


	on spnNbIter changed val do
	(
		btnCreate.caption = "Break in " + ((spnNbParts.value ^ val) as string)
		cbKeepGen.enabled = val != 1
	)
	

	on rltFractureVoronoi close do
	(
		enableSceneRedraw()
		CompleteRedraw()
		callbacks.removeScripts id:#FVcbID01
	)

)	-- end rollout rltFractureVor


createDialog rltFractureVoronoi 126 375 60 130

)	-- end script
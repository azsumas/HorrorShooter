﻿///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                                                                                                                                             ///
///     MIT License                                                                                                                                             ///
///                                                                                                                                                             ///
///     Copyright (c) 2016 Raphaël Ernaelsten (@RaphErnaelsten)                                                                                                 ///
///                                                                                                                                                             ///
///     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),      ///
///     to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute,                  ///
///     and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:              ///
///                                                                                                                                                             ///
///     The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                          ///
///                                                                                                                                                             ///
///     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,     ///
///     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER      ///
///     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS    ///
///     IN THE SOFTWARE.                                                                                                                                        ///
///                                                                                                                                                             ///
///     PLEASE CONSIDER CREDITING AURA IN YOUR PROJECTS. IF RELEVANT, USE THE UNMODIFIED LOGO PROVIDED IN THE "LICENSE" FOLDER.                                 ///
///                                                                                                                                                             ///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace AuraAPI
{
    /// <summary>
    /// Collection of function/variables related to the Mesh class
    /// </summary>
    public static class MeshHelpers
    {
        #region Private Members
        /// <summary>
        /// Just a Cube
        /// </summary>
        private static Mesh _cube;
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the Cube mesh
        /// </summary>
        public static Mesh Cube
        {
            get
            {
                if(MeshHelpers._cube == null)
                {
                    MeshHelpers._cube = MeshHelpers.CreateCubeMesh(1, 1, 1);
                }

                return MeshHelpers._cube;
            }
        }

        /// <summary>
        /// Creates a scaled Cube (http://wiki.unity3d.com/index.php/ProceduralPrimitives)
        /// </summary>
        /// <param name="width">Size of the cube along the width</param>
        /// <param name="height">Size of the cube along the height</param>
        /// <param name="length">Size of the cube along the length</param>
        /// <returns>A mesh representing the scaled cube</returns>
        public static Mesh CreateCubeMesh(float width, float height, float length)
        {
            #region Vertices
            Vector3 p0 = new Vector3(-width * .5f, -height * .5f, length * .5f);
            Vector3 p1 = new Vector3(width * .5f, -height * .5f, length * .5f);
            Vector3 p2 = new Vector3(width * .5f, -height * .5f, -length * .5f);
            Vector3 p3 = new Vector3(-width * .5f, -height * .5f, -length * .5f);

            Vector3 p4 = new Vector3(-width * .5f, height * .5f, length * .5f);
            Vector3 p5 = new Vector3(width * .5f, height * .5f, length * .5f);
            Vector3 p6 = new Vector3(width * .5f, height * .5f, -length * .5f);
            Vector3 p7 = new Vector3(-width * .5f, height * .5f, -length * .5f);

            Vector3[] vertices =
            {
                // Bottom
                p0,
                p1,
                p2,
                p3,

                // Left
                p7,
                p4,
                p0,
                p3,

                // Front
                p4,
                p5,
                p1,
                p0,

                // Back
                p6,
                p7,
                p3,
                p2,

                // Right
                p5,
                p6,
                p2,
                p1,

                // Top
                p7,
                p6,
                p5,
                p4
            };
            #endregion

            #region Normales
            Vector3 up = Vector3.up;
            Vector3 down = Vector3.down;
            Vector3 front = Vector3.forward;
            Vector3 back = Vector3.back;
            Vector3 left = Vector3.left;
            Vector3 right = Vector3.right;

            Vector3[] normales =
            {
                // Bottom
                down,
                down,
                down,
                down,

                // Left
                left,
                left,
                left,
                left,

                // Front
                front,
                front,
                front,
                front,

                // Back
                back,
                back,
                back,
                back,

                // Right
                right,
                right,
                right,
                right,

                // Top
                up,
                up,
                up,
                up
            };
            #endregion

            #region UVs
            Vector2 _00 = new Vector2(0f, 0f);
            Vector2 _10 = new Vector2(1f, 0f);
            Vector2 _01 = new Vector2(0f, 1f);
            Vector2 _11 = new Vector2(1f, 1f);

            Vector2[] uvs =
            {
                // Bottom
                _11,
                _01,
                _00,
                _10,

                // Left
                _11,
                _01,
                _00,
                _10,

                // Front
                _11,
                _01,
                _00,
                _10,

                // Back
                _11,
                _01,
                _00,
                _10,

                // Right
                _11,
                _01,
                _00,
                _10,

                // Top
                _11,
                _01,
                _00,
                _10
            };
            #endregion

            #region Triangles
            int[] triangles =
            {
                // Bottom
                3,
                1,
                0,
                3,
                2,
                1,

                // Left
                3 + 4 * 1,
                1 + 4 * 1,
                0 + 4 * 1,
                3 + 4 * 1,
                2 + 4 * 1,
                1 + 4 * 1,

                // Front
                3 + 4 * 2,
                1 + 4 * 2,
                0 + 4 * 2,
                3 + 4 * 2,
                2 + 4 * 2,
                1 + 4 * 2,

                // Back
                3 + 4 * 3,
                1 + 4 * 3,
                0 + 4 * 3,
                3 + 4 * 3,
                2 + 4 * 3,
                1 + 4 * 3,

                // Right
                3 + 4 * 4,
                1 + 4 * 4,
                0 + 4 * 4,
                3 + 4 * 4,
                2 + 4 * 4,
                1 + 4 * 4,

                // Top
                3 + 4 * 5,
                1 + 4 * 5,
                0 + 4 * 5,
                3 + 4 * 5,
                2 + 4 * 5,
                1 + 4 * 5
            };
            #endregion

            Mesh mesh = new Mesh
                        {
                            vertices = vertices,
                            normals = normales,
                            uv = uvs,
                            triangles = triangles
                        };
            mesh.RecalculateBounds();

            return mesh;
        }
        #endregion
    }
}

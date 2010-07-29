/* Dust -- Copyright (C) 2009-2010 GameClay LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

__declspec(dllexport) void __cdecl AdvanceTime(
   float dt,
   float* pX_stream,
   float* pY_stream,
   float* pZ_stream,
   float* vX_stream,
   float* vY_stream,
   float* vZ_stream,
   float* time_stream,
   int* num_particles
   )
{
   const int num = *num_particles;
   int i;
   for(i = 0; i < num; i++)
   {
      *time_stream -= dt;
      *time_stream++;

      *pX_stream += *vX_stream * dt;
      *pX_stream += *vX_stream * dt;
      *pX_stream += *vX_stream * dt;

      pX_stream++;
      pY_stream++;
      pZ_stream++;

      vX_stream++;
      vY_stream++;
      vZ_stream++;
   }
}
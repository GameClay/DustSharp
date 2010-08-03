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
#ifndef NO_SIMD
#  include <xmmintrin.h>
#endif

#ifdef __WIN32
__declspec(dllexport) void __cdecl
#endif
AdvanceTime(
   float dt,
   float* pX_stream,
   float* pY_stream,
   float* pZ_stream,
   float* vX_stream,
   float* vY_stream,
   float* vZ_stream,
   float* time_stream,
   int num_particles
   )
{
   int i;

#ifndef NO_SIMD
   // Temporarys (should fit registers)
   __m128 dt_v, ts_reg, vX_reg, vY_reg, vZ_reg, 
      pX_reg, pY_reg, pZ_reg, zero_v,
      tX_reg, tY_reg, tZ_reg;
    
   // Load dt
   dt_v = _mm_set1_ps(dt);
   
   // Load zero
   zero_v = _mm_setzero_ps();
   
   int numBatches = num_particles / 4;
   int numRemaining = num_particles % 4;
   
   // Process batches
   for(i = 0; i < numBatches; i++)
   {
      ts_reg = _mm_loadu_ps(time_stream);
      
      vX_reg = _mm_loadu_ps(vX_stream); vX_stream += 4;
      vY_reg = _mm_loadu_ps(vY_stream); vY_stream += 4;
      vZ_reg = _mm_loadu_ps(vZ_stream); vZ_stream += 4;
      
      // Decrement time remaining
      ts_reg = _mm_max_ps(_mm_sub_ps(ts_reg, dt_v), zero_v);
      
      pX_reg = _mm_loadu_ps(pX_stream);
      pY_reg = _mm_loadu_ps(pY_stream);
      pZ_reg = _mm_loadu_ps(pZ_stream);
      
      // Update position from velocity
      tX_reg = _mm_mul_ps(vX_reg, dt_v);
      tY_reg = _mm_mul_ps(vY_reg, dt_v);
      tZ_reg = _mm_mul_ps(vZ_reg, dt_v);
      
      // Use velocity as temp now (no drag right now)
      vX_reg = _mm_add_ps(tX_reg, pX_reg);
      vY_reg = _mm_add_ps(tY_reg, pY_reg);
      vZ_reg = _mm_add_ps(tZ_reg, pZ_reg);
      
      // Store time and position
      _mm_storeu_ps(pX_stream, vX_reg); pX_stream += 4;
      _mm_storeu_ps(pY_stream, vY_reg); pY_stream += 4;
      _mm_storeu_ps(pZ_stream, vZ_reg); pZ_stream += 4;
      _mm_storeu_ps(time_stream, ts_reg); time_stream += 4;
   }
#else
   int numRemaining = num_particles;
#endif

   // Process remaining individuals
   for(i = 0; i < numRemaining; i++)
   {
      *time_stream -= dt;
      *time_stream++;

      *pX_stream += *vX_stream * dt;
      *pY_stream += *vY_stream * dt;
      *pZ_stream += *vZ_stream * dt;

      pX_stream++;
      pY_stream++;
      pZ_stream++;

      vX_stream++;
      vY_stream++;
      vZ_stream++;
   }
}
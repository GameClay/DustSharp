
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
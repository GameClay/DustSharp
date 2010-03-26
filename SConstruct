from SCons.Util import flatten
import os

env = Environment(tools=['default', 'mono'])
   
effecters = Glob('effecter/*.cs', strings=True)
emitters = Glob('emitter/*.cs', strings=True)
game_objects = Glob('gameObjects/*.cs', strings=True)
parameters = Glob('parameter/*.cs', strings=True)
representations = Glob('representation/*.cs', strings=True)

excluded_files = effecters + emitters + game_objects + parameters + representations

dust_files = Glob('*.cs', strings=True)
dust_files = [f for f in dust_files if f not in excluded_files]

dust_cli = env.CLILibrary('GameClay.Dust.dll',
	dust_files,
	CSCLIBFLAGS=[]
)
Return('dust_cli')
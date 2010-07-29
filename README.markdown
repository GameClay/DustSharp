# DustSharp

## Overview
Coming soon, to an overview near you...the overview!

## License
Unless otherwise stated in a file, DustSharp is licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0). 

Each code file in DustSharp has its license located at the top of the file. If any file is missing its license, it should be considered to be licensed under the Apache License, Version 2.0. 

### Apache License, Version 2.0
The full text of the license may be found in the LICENSE file, as well as the Apache site: http://www.apache.org/licenses/LICENSE-2.0
> Dust -- Copyright (C) 2009-2010 GameClay LLC
>
> Licensed under the Apache License, Version 2.0 (the "License");
> you may not use this file except in compliance with the License.
> You may obtain a copy of the License at
>
>     http://www.apache.org/licenses/LICENSE-2.0
>
> Unless required by applicable law or agreed to in writing, software
> distributed under the License is distributed on an "AS IS" BASIS,
> WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
> See the License for the specific language governing permissions and
> limitations under the License.

## Updating from Dust
	git clone Dust DustSharp
	git filter-branch --subdirectory-filter DustSharp
	git remote rm origin
	git remote add origin git@github.com:GameClay/DustSharp.git
	git push origin master
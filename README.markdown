# DustSharp

## Overview
Coming soon, to an overview near you...the overview!

## License
Unless otherwise stated in a file, DustSharp is licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0), the text of which can be found at the previous link, or in the LICENSE file.

Each code file in DustSharp has its license located at the top of the file. If any file is missing its license, it should be considered to be licensed under the Apache License, Version 2.0, 

### GPL Compatibility
DustSharp may be included with projects which use version 3 of the GPL. Please see [Apache License v2.0 and GPL Compatibility](http://www.apache.org/licenses/GPL-compatibility.html) for more details.

### Dual-Licensing
GameClay has no plans to dual-license DustSharp.

## Updating from Dust
This set of commands allows authors to update the public-facing DustSharp repository from the Dust technology base.
	git clone Dust DustSharp
	git filter-branch --subdirectory-filter DustSharp
	git remote rm origin
	git remote add origin git@github.com:GameClay/DustSharp.git
	git push origin master
# DustSharp

## Overview
Coming soon, to an overview near you...

## License
The code contained in DustSharp is protected by one of two licenses: [LGPL](http://www.gnu.org/licenses/lgpl-2.1.html), or [MIT license](http://en.wikipedia.org/wiki/MIT_License). In general, code which is core to the functionality of DustSharp is protected by the LGPL; while code which implements or integrates the core-functionality is released under the MIT license.

Each code file in DustSharp has its license located at the top of the file.

If any file located inside the DustSharp repository is missing a license notification, please inform licensing@gameclay.com, and treat the file as though it were licensed under the LGPL.

A given revision of a given file is covered by the license specified in that revision of that file. In other words, if a file starts out MIT, and is changed to LGPL, revisions of that file which contain the MIT license at the top continue to be available under the MIT license, while revisions of that file which contain the LGPL at the top are protected by the LGPL.

DustSharp is available under other licenses. Please contact: licensing@gameclay.com for more information.

### LGPL
Portions of DustSharp are protected under version 2.1 of the Lesser GNU Public License.

* A user of distributed software made with DustSharp _must_ be able to replace the DustSharp library with a version they have compiled from source. 
* If you distribute software made with an altered version of DustSharp, you _must_ make available any code changes made to DustSharp.

If you do not, or can not, meet these requirements, contact licensing@gameclay.com to inquire about alternate licensing.

This is only an overview of the LGPL, for complete information, please see: http://www.gnu.org/licenses/lgpl-2.1.html

### MIT
Portions of DustSharp are licensed under the [MIT license](http://en.wikipedia.org/wiki/MIT_License).

These files will have the following located at the top of the file:
> Copyright (c) 2009-2010 GameClay LLC
>
> Permission is hereby granted, free of charge, to any person
> obtaining a copy of this software and associated documentation
> files (the "Software"), to deal in the Software without
> restriction, including without limitation the rights to use,
> copy, modify, merge, publish, distribute, sublicense, and/or sell
> copies of the Software, and to permit persons to whom the
> Software is furnished to do so, subject to the following
> conditions:
>
> The above copyright notice and this permission notice shall be
> included in all copies or substantial portions of the Software.
>
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
> EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
> OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
> NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
> HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
> WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
> FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
> OTHER DEALINGS IN THE SOFTWARE.

## Updating from Dust
	git clone Dust DustSharp
	git filter-branch --subdirectory-filter DustSharp
	git remote rm origin
	git add origin git@github.com/GameClay/DustSharp.git
	git push origin master
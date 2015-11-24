# BBCode Orchard module Readme



## Project Description

Makes the usage of various BBCodes available by adding a new text flavor (bbcode)
You can install the module from the Gallery.


## Documentation

### Usage

After installing and enabling the module, you can use the filter in the following ways. Note that either way, if you want to have the default styling the module includes you have to include the piedone-bbcode-styles.css stylesheet (or the "BBCode" resource) on the pages you have BB codes.

### As a Body flavor

You can set the flavor to "bbcode" (case-insensitive) on the Body of the contenty type(s) you want to have BBCode support.

### As a filter on arbitrary strings

Just request an IBBCodeFilter instance in the constructor of the class you want to use it (this is standard dependency injection in Orchard) and use the object's `Parse()` method.  
**Note that the string to be parsed is html encoded before parsing, therefore the result will be html encoded too!**

### As an html helper

Add the `@using Piedone.BBCode.Extensions;` directive to the top of the Razor view. Then you can use the new `Html.ParseBBCode()` helper to parse BB codes in-place.  
**Note that the string to be parsed is html encoded before parsing, therefore the result will be html encoded too!**


## Supported tags

The default tags are: b, i, u, s, code, img, quote, sup, sub, url (all in the standard BBCode form). Use the [BBCode test text](Docs/BBCodeTestText.md) to test the parser.
You can add new tags or remove defaults with the IBBCodeFilter's `AddTag()` and `RemoveTag()` methods. Since the filter uses [Codekicker.BBCode](http://bbcode.codeplex.com) internally, you have to supply `AddTag()` with a BBTag object.


## Styling

Just look at the default stylesheet included in the Styles folder. Override the styles in your theme's stylesheet as you like.

[Version History](Docs/VersionHistory.md)

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-bbcode](https://bitbucket.org/Lombiq/orchard-bbcode) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-BBCode](https://github.com/Lombiq/Orchard-BBCode) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.
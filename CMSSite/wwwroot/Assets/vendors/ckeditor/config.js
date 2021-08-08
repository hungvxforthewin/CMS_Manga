/**
* @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
* For licensing, see LICENSE.md or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function(config) {
    // Define changes to default configuration here.
    // For the complete reference:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config
    
    // The toolbar groups arrangement, optimized for two toolbar rows.
    //config.toolbarGroups = [
    //	{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
    //	{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
    //	{ name: 'links' },
    //	{ name: 'insert' },
    //	{ name: 'forms' },
    //	{ name: 'tools' },
    //	{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
    //	{ name: 'others' },
    //	'/',
    //	{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
    //	{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
    //	{ name: 'styles', groups: ["Styles", "Format"] },
    //	{ name: 'colors', groups: ['TextColor', 'BGColor'] },
    //	{ name: 'about' }
    //];
    //config.toolbar =
    //                    [
    //                        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
		  //                  { name: 'editing', items: ['Scayt'] },
		  //                  { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
		  //                  { name: 'insert', items: ['Table', 'HorizontalRule', 'SpecialChar'] },
		  //                  { name: 'tools', items: ['Maximize'] },
		  //                  { name: 'document', items: ['Source'] },
		  //                  '/',
		  //                  { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
		  //                  { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
		  //                  { name: 'styles', items: ['Styles', 'Format'] },
    //                    ];

    // Remove some buttons, provided by the standard plugins, which we don't
    // need to have in the Standard(s) toolbar.
    //config.removeButtons = 'Strike,Subscript,Superscript,CreateDiv';
    // Se the most common block elements.
    //config.format_tags = 'p;h1;h2;h3;pre';

    // Make dialogs simpler.
   // config.removeDialogTabs = 'image:advanced;link:advanced';
    config.language = 'vi';
    //config.filebrowserBrowseUrl = '/Assets/plugins/ckfinder/ckfinder.html';
    //config.filebrowserImageBrowseUrl = '/Assets/plugins/ckfinder/ckfinder.html?type=Images';
    //config.filebrowserFlashBrowseUrl = '/Assets/plugins/ckfinder/ckfinder.html?type=Flash';
    //config.filebrowserUploadUrl = '/Assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    //config.filebrowserImageUploadUrl = '/Assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    //config.filebrowserFlashUploadUrl = '/Assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';


    //CKFinder.setupCKEditor(null, '/ckfinder/');

    //FCKConfig.LinkBrowserURL = FCKConfig.BasePath + 'filemanager/browser/default/browser.html?Type=File&Connector=../../../../connector.' + _FileBrowserExtension;

    //FCKConfig.LinkUploadURL = FCKConfig.BasePath + '../ckfinder.' + _QuickUploadExtension + '?Type=File';

};



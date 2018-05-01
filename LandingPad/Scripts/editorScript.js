(function () {
    $(document).ready(function () {
        var fonts = ['arial', 'times', 'courier'];
        var Font = Quill.import('formats/font');
        Font.whitelist = fonts;
        Quill.register(Font, true);

        var editor = new Quill('#editorContainer .editor', {
            bounds: '#editorContainer .editor',
            modules: {
                'formula': true,
                'syntax': true,
                'toolbar': [
                    [{ 'font': fonts }, { 'size': [] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    [{ 'color': [] }, { 'background': [] }],
                    [{ 'script': 'super' }, { 'script': 'sub' }],
                    [{ 'header': '1' }, { 'header': '2' }, 'blockquote', 'code-block'],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'indent': '-1' }, { 'indent': '+1' }],
                    [{ 'align': [] }],
                    ['link', 'image'],
                    ['clean']
                ],
            },
            placeholder: 'Write your own destiny...',
            name: 'EditorContent',
            theme: 'snow'
        });

        loadFonts();
    });

    function loadFonts() {
        window.WebFontConfig = {
            google: { families: ['Inconsolata::latin', 'Ubuntu+Mono::latin', 'Slabo+27px::latin', 'Roboto+Slab::latin'] }
        };
        (function () {
            var wf = document.createElement('script');
            wf.src = 'https://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';
            wf.type = 'text/javascript';
            wf.async = 'true';
            var s = document.getElementsByTagName('script')[0];
            s.parentNode.insertBefore(wf, s);
        })();
    }
})();
// to Form Data
function toFormData(obj, form, namespace) {
    let fd = form || new FormData();
    let formKey;

    for (let property in obj) {
        if (obj.hasOwnProperty(property) && obj[property]) {
            if (namespace && Array.isArray(obj)) {
                formKey = namespace + '[' + property + ']';
            }
            else if (namespace) {
                formKey = namespace + '.' + property;
            }
            else {
                formKey = property;
            }

            // if the property is an object, but not a File, use recursivity.
            if (obj[property] instanceof Date) {
                fd.append(formKey, obj[property].toISOString());
            }
            else if (typeof obj[property] === 'object' && !(obj[property] instanceof File)) {
                toFormData(obj[property], fd, formKey);
            } else { // if it's a string or a File object
                fd.append(formKey, obj[property]);
            }
        }
    }
    return fd;
}

// Slugify
String.prototype.removeAcento = function () {
	var text = this.toLowerCase();
	text = text.replace(new RegExp('[ÁÀÂÃ]', 'gi'), 'a');
	text = text.replace(new RegExp('[ÉÈÊ]', 'gi'), 'e');
	text = text.replace(new RegExp('[ÍÌÎ]', 'gi'), 'i');
	text = text.replace(new RegExp('[ÓÒÔÕ]', 'gi'), 'o');
	text = text.replace(new RegExp('[ÚÙÛÜ]', 'gi'), 'u');
	text = text.replace(new RegExp('[Ç]', 'gi'), 'c');
	text = text.replace(new RegExp('[ç]', 'gi'), 'c');
	text = text.replace(new RegExp('[Ə]', 'gi'), 'e');
	text = text.replace(new RegExp('[ə]', 'gi'), 'e');
	text = text.replace(new RegExp('[I]', 'gi'), 'i');
	text = text.replace(new RegExp('[ı]', 'gi'), 'i');
	text = text.replace(new RegExp('[Ğ]', 'gi'), 'g');
	text = text.replace(new RegExp('[ğ]', 'gi'), 'g');
	text = text.replace(new RegExp('[Ş]', 'gi'), 'sh');
	text = text.replace(new RegExp('[ş]', 'gi'), 'sh');
	return text;
};
String.prototype.slugify = function () {
	return this.toString().toLowerCase().removeAcento().trim()
		.replace(/\s+/g, '-')           // Replace spaces with -
		.replace(/[^\w\-]+/g, '')       // Remove all non-word chars
		.replace(/\-\-+/g, '-')         // Replace multiple - with single -
		.replace(/^-+/, '')             // Trim - from start of text
		.replace(/-+$/, '');            // Trim - from end of text
};
$("input[data-type='slug-source']").blur(function () {
	$("input[data-type='slug-input']").val($(this).val().slugify());
});
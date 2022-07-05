$("#message-box").css("display", "block");
setTimeout(function () { $("#message-box").hide('slow'); }, 5000);

function cargarListas(urlAction, model, divTarget, selectedValue) {
    $(divTarget).empty();
    $.ajax({
        url: urlAction,
        type: 'POST',
        dataType: 'json',
        data: model,
        success: function (data) {
            if (data) {
                $.each(data, function (i, item) {
                    if (item.Value === selectedValue) {
                        $(divTarget).append('<option value="' + item.Value + '" selected="selected">' + item.Text + '</option>');
                    } else {
                        $(divTarget).append('<option value="' + item.Value + '">' + item.Text + '</option>');
                    }
                });
            }
        },
        error: function (ex) {
            alert('Error recuperando los elementos.' + ex);
        }
    });
}

function eliminarDocumentos(token, nombreArchivo, urlAction) {
    if (confirm("¿Está seguro de que desea eliminar el documento '" + nombreArchivo + "' de forma permanente?")) {
        $.ajax({
            url: urlAction,
            type: 'POST',
            dataType: 'json',
            data: { id: token },
            success: function () {
                $(".class" + token).remove();
            },
            error: function (ex) {
                alert('Error eliminando el documento.' + ex);
            }
        });
    }
}
var Alto = [
    { 'codMunicipio': '19050', 'Municipio': 'Argelia' },
    { 'codMunicipio': '19075', 'Municipio': 'Balboa' },
    { 'codMunicipio': '19110', 'Municipio': 'Buenos Aires' },
    { 'codMunicipio': '19130', 'Municipio': 'Cajibío' },
    { 'codMunicipio': '19137', 'Municipio': 'Caldono' },
    { 'codMunicipio': '19142', 'Municipio': 'Caloto' },
    { 'codMunicipio': '19212', 'Municipio': 'Corinto' },
    { 'codMunicipio': '52233', 'Municipio': 'Cumbitara' },
    { 'codMunicipio': '52256', 'Municipio': 'El Rosario' },
    { 'codMunicipio': '19256', 'Municipio': 'El Tambo' },
    { 'codMunicipio': '76275', 'Municipio': 'Florida' },
    { 'codMunicipio': '19364', 'Municipio': 'Jambaló' },
    { 'codMunicipio': '52405', 'Municipio': 'Leiva' },
    { 'codMunicipio': '52418', 'Municipio': 'Los Andes' },
    { 'codMunicipio': '19450', 'Municipio': 'Mercaderes' },
    { 'codMunicipio': '19455', 'Municipio': 'Miranda' },
    { 'codMunicipio': '19473', 'Municipio': 'Morales' },
    { 'codMunicipio': '19532', 'Municipio': 'Patía' },
    { 'codMunicipio': '19548', 'Municipio': 'Piendamó' },
    { 'codMunicipio': '52540', 'Municipio': 'Policarpa' },
    { 'codMunicipio': '76563', 'Municipio': 'Pradera' },
    { 'codMunicipio': '19698', 'Municipio': 'Santander de Quilichao' },
    { 'codMunicipio': '19780', 'Municipio': 'Suárez' },
    { 'codMunicipio': '19821', 'Municipio': 'Toribío' },
];

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

function descargarDocumentos(token, urlAction) {
        $.ajax({
            url: urlAction,
            type: 'POST',
            dataType: 'json',
            data: { id: token },
            success: function () {
            },
            error: function (ex) {
                alert('Error descargando el documento abc.' + ex);
            }
        });
}
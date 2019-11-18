function removerEndereco(id) {
    if (confirm('Tem certeza que deseja remover este endereço?')) {
        $.ajax({
            type: 'POST',
            url: '/Cliente/RemoverEndereco/' + id,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                var node = document.getElementById('divEndereco_' + id);
                if (node.parentNode) {
                    node.parentNode.removeChild(node);
                }

                alert(response.value);
            },
            failure: function () {
                alert('Desculpe. Não foi possível remover o endereço, por favor, tente novamente...');
            }
        });
    }
}
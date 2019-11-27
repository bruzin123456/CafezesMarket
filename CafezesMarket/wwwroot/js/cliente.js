document.addEventListener("DOMContentLoaded", function (event) {
    $('#adicionarEnderecoModal').on('hidden.bs.modal', function (e) {
        $(this).find('#form-endereco')[0].reset();
    });
});

function removerEndereco(id) {
    if (confirm('Tem certeza que deseja remover este endereço?')) {
        $.ajax({
            type: 'DELETE',
            url: '/Cliente/Endereco/' + id,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                var node = document.getElementById('divEndereco_' + id);
                if (node.parentNode) {
                    node.parentNode.removeChild(node);
                }

                alert(response.value);
            },
            error: function () {
                alert('Desculpe. Não foi possível remover o endereço, por favor, tente novamente...');
            },
            failure: function () {
                alert('Desculpe. Não foi possível remover o endereço, por favor, tente novamente...');
            }
        });
    }
}

function adicionarEndereco() {
    var endereco = {
        apelido: $('#inputApelido').val(),
        logradouro: $('#inputLogradouro').val(),
        numero: parseInt($('#inputNumero').val(), 10) || 0,
        complemento: $('#inputComplemento').val(),
        cidade: $('#inputCidade').val(),
        estado: $('#inputEstado').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Cliente/Endereco',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('XSRF-TOKEN',
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        cache: false,
        data: JSON.stringify(endereco),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            alert(response.value);

            window.location.reload(false); 
        },
        error: function (response) {
            if (response.status === 400) {
                alert(response.responseJSON.value.join('\n'));
            } else {
                alert('Desculpe. Não foi possível cadastrar o endereço, por favor, tente novamente...');
            }
        },
        failure: function () {
            alert('Desculpe. Não foi possível cadastrar o endereço, por favor, tente novamente...');
        }
    });
}
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function adicionarAoCarrinho(produtoId, quant) {
    var carrinhoItem = {
        produto: produtoId,
        quantidade: quant
    };

    $.ajax({
        type: 'POST',
        url: '/Carrinho/Adicionar',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('XSRF-TOKEN',
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        cache: false,
        data: JSON.stringify(carrinhoItem),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            alert(response.value);

            window.location.reload(false);
        },
        error: function (response) {
            if (response.status != null && response.status === 400) {
                alert(response.responseJSON.value.join('\n'));
            } else if (response.status === 401) {
                alert('Preicsa estar logado');
            } else {
                alert('Desculpe. Não foi possível adicionar ao carrinho, por favor, tente novamente...');
            }
        },
        failure: function () {
            alert('Desculpe. Não foi possível adicionar ao carrinho, por favor, tente novamente...');
        }
    });
}

function removerDoCarrinho(id) {
    if (confirm('Tem certeza que deseja remover do carrinho ?')) {
        $.ajax({
            type: 'DELETE',
            url: '/Carrinho/Remover/' + id,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('XSRF-TOKEN',
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                alert(response.value);
                window.location.reload(false);
            },
            error: function () {
                alert('Desculpe. Não foi possível remover o item do carrinho, por favor, tente novamente...');
            },
            failure: function () {
                alert('Desculpe. Não foi possível remover o item do carrinho, por favor, tente novamente...');
            }
        });
    }
}
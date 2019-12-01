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
            } else {
                alert('Desculpe. Não foi possível adicionar ao carrinho, por favor, tente novamente...');
            }
        },
        failure: function () {
            alert('Desculpe. Não foi possível adicionar ao carrinho, por favor, tente novamente...');
        }
    });
}

function adicionarProduto(produtoId){
    var quantidade = parseInt($('#inputQuantidade').val());
    adicionarAoCarrinho(produtoId, quantidade);
};
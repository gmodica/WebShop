function CatalogViewModel(url) {
    var self = this;

    self.products = ko.observableArray();

    function ProductViewModel(root, product) {
        this.id = product.Id;
        this.name = product.Name;
        this.price = product.Price.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
        this.image = product.Image;
    }

    spinner.spin(document.getElementById('spinner'));
    $.getJSON(url, function (products) {
        $.each(products, function (index, product) {
            self.products.push(new ProductViewModel(self, product));
        });
        spinner.stop();
    });
}

function ProductDetailViewModel(url) {
    var self = this;

    self.url = url;

    self.id = ko.observable('');
    self.name = ko.observable('');
    self.description = ko.observable('');
    self.price = ko.observable('');
    self.image = ko.observable('');

    self.update = function (id) {
        $.getJSON(self.url + '/' + id, function (product) {
            self.id(product.Id);
            self.name(product.Name);
            self.description(product.Description);
            self.price(product.Price.toLocaleString('en-US', { style: 'currency', currency: 'USD' }));
            self.image(product.Image);
        });
    }

}
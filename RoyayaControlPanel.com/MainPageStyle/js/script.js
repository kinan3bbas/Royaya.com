$(document).ready(function () {
    roaashow();
});
function roaashow() {
  $.ajax({
        type: "GET",
        url: 'http://abbaskinan-001-site6.ctempurl.com/odata/Dreams',
        dataType: "jsonp",
        success: function (Data) {
        $(Data.value).each(function (index, value) {
           var data= "<div class='col'>"+
              "<div class='card rounded-card  ' style='width: 20rem;'>"+
                "<div class='title text-right '>"+
                 " <p class='pr-4 pt-2'>رؤية مجانية</p>"+
                  "<hr>"+
                  "</div>"+
          "<div class='card-body'>"+
            "<p class='card-text text-right'>نص افتراضينص افتراضينص افتراضينص افتراضينص افتراضينص افتراضينص افتراضي</p>"+
          "</div>"+
          "<hr>"+
          "<div class='d-flex bd-highlight justify-content-between'>"+
            "<div class='p-2 bd-highlight'>15 اكتوبر 2019</div>"+
                "<div class='p-2 flex-shrink-1 bd-highlight'><span class='pl-1'><i class='fas fa-eye text-primary'></i>15</span>"+
                  "<span><i class='fas fa-heart text-danger'></i>15</span>"+
                  "</div>"+
          "</div>"+

        "</div>"+

            "</div>"+
           
            "</div>"
        });
        
  }
});

}


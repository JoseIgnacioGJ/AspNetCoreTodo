// Este código primero usa jQuery (a una librería de apoyo en JavaScript) para
// adjuntar algo de código al evento click de todos las casillas de verificación 
// sobre la página con la clase CSS done - checkbox. Cuando una casilla de 
// verificación es presionada, la función markCompleted() es ejecutada.
$(document).ready(function () {
    $('.done-checkbox').on('click', function (e) {
        markCompleted(e.target);
    });
});

// La función markCompleted() hace algunas cosas:
function markCompleted(checkbox) {

    // Agrega el atributo disabled a las casillas de verificación así 
    // estas no pueden ser selecionadas otra vez
    checkbox.disabled = true;
    
    // Agrega la clase CSS done a la fila padre que contiene la casilla de
    // verificación, la cual cambia la forma que la final luce basada en las
    // reglas CSS en el archivo style.css
    var row = checkbox.closest('tr');
    $(row).addClass('done');

    // Envia el formulario
    var form = checkbox.closest('form');
    form.submit();
}

// Esto toma responsabilidad del la vista y el código del lado del cliente.
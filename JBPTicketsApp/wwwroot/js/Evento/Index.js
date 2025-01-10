const fetchBtn = document.querySelector('#fetch');
const eventosTableBody = document.querySelector('#eventosTable tbody');

const fetchData = async e => {
    try {
        console.log('fetch')
        const response = await fetch('/Eventos/GetEventos', {
            method: 'GET',
        });

        if (!response.ok) {
            throw new Error(`Error en la solicitud: ${response.status} ${response.statusText}`);
        }

        const eventos = await response.json();
        let i = 1

        // Limpiar el contenido existente
        eventosTableBody.innerHTML = '';
        document.querySelector('#eventosTable').classList.remove('d-none')
        // Renderizar los eventos en la tabla
        eventos.forEach(evento => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${i}</td>
                <td>${evento.nombre}</td>
                <td>${new Date(evento.fecha).toLocaleDateString()}</td>
                <td>
                    <a class="btn btn-outline-info" href="AsignarTickets/${evento.idEvento}" title="Asignar tickets">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon icon-tabler icons-tabler-outline icon-tabler-user-plus"><path stroke="none" d="M0 0h24v24H0z" fill="none" /><path d="M8 7a4 4 0 1 0 8 0a4 4 0 0 0 -8 0" /><path d="M16 19h6" /><path d="M19 16v6" /><path d="M6 21v-2a4 4 0 0 1 4 -4h4" /></svg>
                    </a> |
                    <a class="btn btn-outline-success" asp-action="Edit"  asp-route-id="@item.IdEvento" title="Editar">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon icon-tabler icons-tabler-outline icon-tabler-edit"><path stroke="none" d="M0 0h24v24H0z" fill="none" /><path d="M7 7h-1a2 2 0 0 0 -2 2v9a2 2 0 0 0 2 2h9a2 2 0 0 0 2 -2v-1" /><path d="M20.385 6.585a2.1 2.1 0 0 0 -2.97 -2.97l-8.415 8.385v3h3l8.385 -8.415z" /><path d="M16 5l3 3" /></svg>
                    </a> |
                    <a class="btn btn-outline-secondary" href="Details/${evento.idEvento}" title="Detalles del evento">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon icon-tabler icons-tabler-outline icon-tabler-file-description"><path stroke="none" d="M0 0h24v24H0z" fill="none" /><path d="M14 3v4a1 1 0 0 0 1 1h4" /><path d="M17 21h-10a2 2 0 0 1 -2 -2v-14a2 2 0 0 1 2 -2h7l5 5v11a2 2 0 0 1 -2 2z" /><path d="M9 17h6" /><path d="M9 13h6" /></svg>
                    </a> |
                    <a class="btn btn-outline-danger" asp-action="Delete" asp-route-id="@item.IdEvento" title="Eliminar">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon icon-tabler icons-tabler-outline icon-tabler-trash"><path stroke="none" d="M0 0h24v24H0z" fill="none" /><path d="M4 7l16 0" /><path d="M10 11l0 6" /><path d="M14 11l0 6" /><path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" /><path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" /></svg>
                    </a>
                </td>
            `
            i++
                ;
            eventosTableBody.appendChild(row);
        });

    } catch (err) {
        console.error("Error al procesar la solicitud:", err);
        alert("Ocurrió un error al cargar los eventos.");
    } finally{
        document.querySelector('#loader').classList.add('d-none')
    }
}

fetchData()
if (document.querySelectorAll('.show-abonar') || document.querySelectorAll('.show-pagar')) {
    const abonarBtn = document.querySelectorAll('.show-abonar');
    const hideAsideBtn = document.querySelectorAll('.hide-aside');
    const abonarAside = document.querySelector('#abonar-side');

    // Campos del formulario
    const inputResponsable = document.querySelector('#responsable');
    const inputCodigoTicket = document.querySelector('#codigo-ticket');
    const inputMontoAbono = document.querySelector('#monto-abono');
    const formAbonar = document.querySelector('#form-abonar');


    const toggleAside = (show) => {
        if (show) {
            abonarAside.classList.remove('hide-side');
            abonarAside.classList.add('show-side');
        } else {
            abonarAside.classList.remove('show-side');
            abonarAside.classList.add('hide-side');
        }
    };

    abonarBtn.forEach(btn => {
        btn.addEventListener('click', () => {
            const responsable = btn.dataset.responsable;
            const codigoTicket = btn.dataset.codigo;

            inputResponsable.textContent = responsable;
            inputCodigoTicket.value = codigoTicket;
            toggleAside(true);
        });
    });

    hideAsideBtn.forEach(btn => {
        btn.addEventListener('click', () => {
            toggleAside(false);
        });
    });

    // Manejar el envío del formulario
    formAbonar.addEventListener('submit', async (e) => {
        e.preventDefault();

        const codigoTicket = inputCodigoTicket.value;
        const montoAbono = inputMontoAbono.value;

        if (!codigoTicket || !montoAbono) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        try {
            const response = await fetch('/tickets/abonar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    codigoTicket: codigoTicket,
                    montoAbono: parseFloat(montoAbono),
                }),
            });

            if (response.ok) {
                const result = await response.json();
                alert(`Abono exitoso: ${result.message}`);
                abonarAside.classList.remove('show-side');
                abonarAside.classList.add('hide-side');
                location.reload()
            } else {
                const error = await response.json();
                alert(`Error al abonar: ${error.message}`);
            }
        } catch (err) {
            console.error("Error al procesar el abono:", err);
            alert("Ocurrió un error al procesar el abono.");
        }
    });

}

if(document.querySelector('#by-status-pdf')){
    const reportForm = document.querySelector('#by-status-pdf')

    reportForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const idEvento = reportForm.querySelector('#id-evento-pdf').value;
        const status = reportForm.querySelector('#status-pdf').value;
        const categoria = reportForm.querySelector('#categoria-pdf').value;
        const red = reportForm.querySelector('#red-pdf').value;

        try {
            const response = await fetch('/tickets/pdf', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    status: status,
                    categoria: categoria,
                    red: red,
                }),
            });

            if (response.ok) {
                const result = await response.json();
                alert(`Reporte generado: ${result.message}`);
            } else {
                const error = await response.json();
                alert(`Error al generar el reporte: ${error.message}`);
            }
        } catch (err) {
            console.error("Error al procesar el reporte:", err);
            alert("Ocurrió un error al procesar el reporte.");
        }
    });
}
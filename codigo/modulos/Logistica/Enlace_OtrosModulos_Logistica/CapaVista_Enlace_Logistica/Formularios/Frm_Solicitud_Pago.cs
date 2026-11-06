using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaContralador_Enlace_Logistica;
using CapaModelo_Enlace_Logistica.Entidades;

namespace CapaVista_Enlace_Logistica.Formas
{
    public partial class Frm_Solicitud_Pago : Form
    {

        private readonly ClsSolicitudPagoControlador _controlador =
    new ClsSolicitudPagoControlador();
        public Frm_Solicitud_Pago()
        {
            InitializeComponent();

            this.Load += Frm_Solicitud_Pago_Load;

            cboServicioTransporte.SelectionChangeCommitted +=
                cboServicioTransporte_SelectionChangeCommitted;

        }


        private void LimpiarDatosServicio()
        {
            // Datos generales del servicio
            txtNumeroServicio.Clear();
            txtNumeroGuia.Clear();
            txtFechaServicio.Clear();

            // Información del transportista
            txtNombreTransportista.Clear();
            txtNitTransportista.Clear();
            txtDireccionTransportista.Clear();

            // Información del pago
            txtMontoPagar.Clear();
            txtConceptoPago.Clear();

            // Detalle
            dgvDetalleServicio.Rows.Clear();

            // Resumen
            ActualizarResumenSolicitud();
        }


        private void Frm_Solicitud_Pago_Load(object sender, EventArgs e)
        {
            try
            {
                var servicios = _controlador.ObtenerServiciosDisponibles();

                cboServicioTransporte.DataSource = null;
                cboServicioTransporte.DisplayMember = "NumeroGuia";
                cboServicioTransporte.ValueMember = "IdServicio";
                cboServicioTransporte.DataSource = servicios;
                cboServicioTransporte.SelectedIndex = -1;

                LimpiarDatosServicio();

                MessageBox.Show(
                    "Servicios encontrados: " + servicios.Count,
                    "Prueba de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar los servicios:\n\n" + ex.Message,
                    "Solicitud de pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }






        private void cboServicioTransporte_SelectionChangeCommitted(
            object sender, EventArgs e)
        {
            ClsServicioTransporte servicio =
                cboServicioTransporte.SelectedItem as ClsServicioTransporte;

            if (servicio == null)
                return;

            try
            {
                // Información del servicio
                txtNumeroServicio.Text = servicio.IdServicio.ToString();
                txtNumeroGuia.Text = servicio.NumeroGuia ?? "";

                txtFechaServicio.Text = servicio.FechaServicio.HasValue
                    ? servicio.FechaServicio.Value.ToString("dd/MM/yyyy")
                    : "";

                // Información del transportista
                txtNombreTransportista.Text =
                    servicio.NombreTransportista ?? "";

                txtNitTransportista.Text =
                    servicio.NitTransportista ?? "";

                txtDireccionTransportista.Text =
                    servicio.DireccionTransportista ?? "";

                // Información del pago
                txtMontoPagar.Text =
                    servicio.MontoTotal.ToString("N2");

                txtConceptoPago.Text =
                    "Pago por servicio de transporte: " +
                    servicio.TipoServicio;

                lblMonedaMonto.Text = servicio.Moneda ?? "";

                rdbQuetzales.Checked = servicio.Moneda == "GTQ";
                rdbDolares.Checked = servicio.Moneda == "USD";

                // Detalle del servicio
                dgvDetalleServicio.Rows.Clear();

                dgvDetalleServicio.Rows.Add(
                    1,
                    servicio.TipoServicio,
                    servicio.Cantidad,
                    servicio.CostoUnitario.ToString("N2"),
                    servicio.MontoTotal.ToString("N2")
                );

                // Actualizar los datos del resumen
                ActualizarResumenSolicitud();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo mostrar la información del servicio.\n\n" +
                    ex.Message,
                    "Solicitud de pago",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            // 1. Deseleccionar el servicio sin borrar su catálogo.
            cboServicioTransporte.SelectedIndex = -1;

            // 2. Limpiar información del servicio.
            txtNumeroServicio.Clear();
            txtNumeroGuia.Clear();
            txtFechaServicio.Clear();

            // 3. Limpiar información del transportista.
            txtNombreTransportista.Clear();
            txtNitTransportista.Clear();
            txtDireccionTransportista.Clear();

            // 4. Limpiar información bancaria.
            cboBancoBeneficiario.SelectedIndex = -1;
            cboTipoCuenta.SelectedIndex = -1;
            txtNumeroCuenta.Clear();

            // 5. Restablecer la moneda.
            rdbQuetzales.Checked = true;
            rdbDolares.Checked = false;

            // 6. Limpiar el detalle del servicio.
            // Compatible con DataGridView enlazado o manual.
            dgvDetalleServicio.DataSource = null;
            dgvDetalleServicio.Rows.Clear();

            // 7. Limpiar información del pago.
            txtMontoPagar.Clear();
            txtConceptoPago.Clear();
            lblMonedaMonto.Text = "GTQ";

            // 8. Restablecer el resumen de la solicitud.
            lblTextoResumenNumeroServicio.Text = "-";
            lblTextoResumenFechaServicio.Text = "-";
            lblTextoResumenTransportista.Text = "-";
            lblTextoResumenNit.Text = "-";
            lblTextoResumenBanco.Text = "-";
            lblTextoResumenTipoCuenta.Text = "-";
            lblTextoResumenNumeroCuenta.Text = "-";
            lblTextoResumenMoneda.Text = "GTQ";
            lblTextoResumenMonto.Text = "-";
            lblTextoResumenConcepto.Text = "-";

            // 9. Colocar el cursor en el servicio.
            cboServicioTransporte.Focus();
        }



        private void ActualizarResumenSolicitud()
        {
            lblTextoResumenNumeroServicio.Text =
                string.IsNullOrWhiteSpace(txtNumeroServicio.Text)
                ? "-"
                : txtNumeroServicio.Text;

            lblTextoResumenFechaServicio.Text =
                string.IsNullOrWhiteSpace(txtFechaServicio.Text)
                ? "-"
                : txtFechaServicio.Text;

            lblTextoResumenTransportista.Text =
                string.IsNullOrWhiteSpace(txtNombreTransportista.Text)
                ? "-"
                : txtNombreTransportista.Text;

            lblTextoResumenNit.Text =
                string.IsNullOrWhiteSpace(txtNitTransportista.Text)
                ? "-"
                : txtNitTransportista.Text;

            lblTextoResumenBanco.Text =
                cboBancoBeneficiario.SelectedIndex >= 0
                ? cboBancoBeneficiario.Text
                : "-";

            lblTextoResumenTipoCuenta.Text =
                cboTipoCuenta.SelectedIndex >= 0
                ? cboTipoCuenta.Text
                : "-";

            lblTextoResumenNumeroCuenta.Text =
                string.IsNullOrWhiteSpace(txtNumeroCuenta.Text)
                ? "-"
                : txtNumeroCuenta.Text;

            lblTextoResumenMoneda.Text =
                rdbQuetzales.Checked ? "GTQ" :
                rdbDolares.Checked ? "USD" : "-";

            lblTextoResumenMonto.Text =
                string.IsNullOrWhiteSpace(txtMontoPagar.Text)
                ? "-"
                : txtMontoPagar.Text;

            lblTextoResumenConcepto.Text =
                string.IsNullOrWhiteSpace(txtConceptoPago.Text)
                ? "-"
                : txtConceptoPago.Text;
        }




        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pnlFondoSolicitudPago_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblMoneda_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pnlInformacionPago_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSeleccionarDocumentos_Click(object sender, EventArgs e)
        {

        }

        private void lblDocumentosRespaldo_Click(object sender, EventArgs e)
        {

        }

        private void pnlResumenSolicitud_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblResumenNit_Click(object sender, EventArgs e)
        {

        }

        private void lblResumenBanco_Click(object sender, EventArgs e)
        {

        }

        private void lblResumenTipoCuenta_Click(object sender, EventArgs e)
        {

        }

        private void lblResumenNumeroCuenta_Click(object sender, EventArgs e)
        {

        }

        private void lblResumenTransportista_Click(object sender, EventArgs e)
        {

        }

        private void lblResumenMoneda_Click(object sender, EventArgs e)
        {

        }

        private void lblResumenMonto_Click(object sender, EventArgs e)
        {

        }

        private void lblResumenConcepto_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}

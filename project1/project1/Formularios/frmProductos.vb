Public Class frmProductos
    'creación de objetos generales
    Private tablaDatosClase As New DataTable
    Public bandera As New Boolean

    Private Sub frmProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbbBuscar.Items.Add("proDescripcion")
        cbbBuscar.Text = "proDescripcion"
        txtCodigo.Enabled = False
        txtStock.Enabled = False
        txtPrecioCompra.TextAlign = HorizontalAlignment.Right
        txtPrecioVenta.TextAlign = HorizontalAlignment.Right
        txtStock.TextAlign = HorizontalAlignment.Right
        btnCancelar.Visible = True
        txtStock.BackColor = Color.White
        If bandera Then
            btnLimpiar.Visible = True
            txtStock.Text = 0
        Else
            btnLimpiar.Visible = False
        End If
        mostrar()
        limpiar()
    End Sub


    Private Sub mostrar()
        Try
            Dim funcion As New fproducto
            tablaDatosClase = funcion.mostrar
            dataListado.Columns.Item("eliminar").Visible = False

            If tablaDatosClase.Rows.Count <> 0 Then
                dataListado.DataSource = tablaDatosClase
                dataListado.ColumnHeadersVisible = True
            Else
                dataListado.DataSource = Nothing
                dataListado.ColumnHeadersVisible = False
            End If
        Catch evento As Exception
            MsgBox(evento.Message)
        End Try
        buscar()
    End Sub

    Private Sub buscar()
        Try
            Dim conjuntoDatos As New DataSet
            conjuntoDatos.Tables.Add(tablaDatosClase.Copy)
            Dim vistaDatos As New DataView(conjuntoDatos.Tables(0))
            vistaDatos.RowFilter = cbbBuscar.Text & " like '%" & txtBuscar.Text & "%'"
            If vistaDatos.Count <> 0 Then
                dataListado.DataSource = vistaDatos
                ocultarColumna()
            Else
                dataListado.DataSource = Nothing
            End If
        Catch evento As Exception
            MsgBox(evento.Message)
        End Try
    End Sub

    Private Sub ocultarColumna()
        dataListado.Columns(1).Visible = False
        dataListado.Columns(2).Visible = False
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        buscar()
    End Sub

    Private Sub limpiar()
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtPrecioCompra.Text = ""
        txtPrecioVenta.Text = ""
        txtCategoria.Text = ""
        txtCatCodigo.Text = ""
        txtBuscar.Text = ""
        txtStock.Text = 0
        dtpFechaVencimiento.Text = ""
        dtpFechaCompra.Text = ""
        cbbBuscar.Text = "catDescripcion"
        dtpFechaCompra.Value = DateTime.Now
        dtpFechaVencimiento.Value = DateTime.Now.AddDays(15)
        txtDescripcion.Enabled = False
        txtPrecioCompra.Enabled = False
        txtPrecioVenta.Enabled = False
        txtCategoria.Enabled = False
        txtCatCodigo.Enabled = False
        dtpFechaCompra.Enabled = False
        dtpFechaVencimiento.Enabled = False
        btnActualizar.Visible = False
        btnGuardar.Visible = False
        btnEliminar.Visible = False
        btnBuscarCategoria.Visible = False
        txtStock.BackColor = Color.White

        picImagen.Image = Nothing
        picImagen.BackgroundImage = Ventas.My.Resources.Resources.noImagen
        picImagen.SizeMode = PictureBoxSizeMode.StretchImage


        If bandera Then
            btnInsertar.Visible = False
        Else
            btnInsertar.Visible = True
        End If
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        limpiar()
    End Sub

    Private Sub dataListado_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellClick
        txtCodigo.Text = dataListado.SelectedCells.Item(1).Value
        txtCatCodigo.Text = dataListado.SelectedCells.Item(2).Value
        txtDescripcion.Text = dataListado.SelectedCells.Item(3).Value
        txtCategoria.Text = dataListado.SelectedCells.Item(4).Value
        txtPrecioVenta.Text = dataListado.SelectedCells.Item(5).Value
        txtPrecioCompra.Text = dataListado.SelectedCells.Item(6).Value
        dtpFechaCompra.Value = dataListado.SelectedCells.Item(7).Value
        dtpFechaVencimiento.Value = dataListado.SelectedCells.Item(8).Value
        txtStock.Text = dataListado.SelectedCells.Item(9).Value
        picImagen.BackgroundImage = Nothing
        Dim b() As Byte = dataListado.SelectedCells.Item(10).Value
        Dim almacenamientoImagen As New IO.MemoryStream(b)
        picImagen.Image = Image.FromStream(almacenamientoImagen)
        picImagen.SizeMode = PictureBoxSizeMode.StretchImage
        If txtStock.Text = 0 Then
            txtStock.BackColor = Color.Red
        Else
            txtStock.BackColor = Color.White
        End If

        If bandera Then
            btnActualizar.Visible = False
        Else
            btnActualizar.Visible = True
        End If
        btnInsertar.Visible = False
        habilitar()
    End Sub

    Private Sub habilitar()
        txtDescripcion.Enabled = True
        txtPrecioCompra.Enabled = True
        txtPrecioVenta.Enabled = True
        txtCategoria.Enabled = True
        txtCatCodigo.Enabled = True
        dtpFechaCompra.Enabled = True
        dtpFechaVencimiento.Enabled = True
        btnBuscarCategoria.Visible = True
        txtStock.Enabled = True
    End Sub

    Private Sub btnInsertar_Click(sender As Object, e As EventArgs) Handles btnInsertar.Click
        limpiar()
        habilitar()
        btnGuardar.Visible = True
        btnInsertar.Visible = False
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtDescripcion.Text <> "" And
            txtPrecioCompra.Text <> "" And
            txtPrecioVenta.Text <> "" Then
            Try
                Dim tablaDatos As New eproducto
                Dim funcion As New fproducto
                Dim almacenamientoImagen As New IO.MemoryStream()
                If Not picImagen.Image Is Nothing Then
                    picImagen.Image.Save(almacenamientoImagen, picImagen.Image.RawFormat)
                Else
                    picImagen.Image = Ventas.My.Resources.Resources.noImagen
                    picImagen.Image.Save(almacenamientoImagen, picImagen.Image.RawFormat)
                End If
                tablaDatos.pcatCodigo = txtCatCodigo.Text
                tablaDatos.pproDescripcion = txtDescripcion.Text
                tablaDatos.pproPrecioCompra = txtPrecioCompra.Text
                tablaDatos.pproFechaCompra = dtpFechaCompra.Value
                tablaDatos.pproPrecioVenta = txtPrecioVenta.Text
                tablaDatos.pproFechaVencimiento = dtpFechaVencimiento.Value
                tablaDatos.pproStock = txtStock.Text
                tablaDatos.pproImagen = almacenamientoImagen.GetBuffer
                If funcion.insertar(tablaDatos) Then
                    MessageBox.Show("Producto fue registrado correctamente",
                                    "Guardando registro", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Producto no fue registrado correctamente",
                                    "Guardando registro", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                End If
                mostrar()
                limpiar()
            Catch evento As Exception
                MsgBox(evento.Message)
            End Try
        Else
            MessageBox.Show("Falta informacion para almacenar",
                            "Guardando registro", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        If txtDescripcion.Text <> "" And
            txtPrecioCompra.Text <> "" And
            txtPrecioVenta.Text <> "" Then
            Dim resultado As DialogResult
            resultado = MessageBox.Show("Desea modificar los datos",
                            "Actualizando registro", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question)
            If resultado = Windows.Forms.DialogResult.OK Then
                Try
                    Dim tablaDatos As New eproducto
                    Dim funcion As New fproducto
                    Dim almacenamientoImagen As New IO.MemoryStream()
                    If Not picImagen.Image Is Nothing Then
                        picImagen.Image.Save(almacenamientoImagen, picImagen.Image.RawFormat)
                    Else
                        picImagen.Image = Ventas.My.Resources.Resources.noImagen
                        picImagen.Image.Save(almacenamientoImagen, picImagen.Image.RawFormat)
                    End If
                    tablaDatos.pproCodigo = txtCodigo.Text
                    tablaDatos.pcatCodigo = txtCatCodigo.Text
                    tablaDatos.pproDescripcion = txtDescripcion.Text
                    tablaDatos.pproPrecioCompra = txtPrecioCompra.Text
                    tablaDatos.pproFechaCompra = dtpFechaCompra.Value
                    tablaDatos.pproPrecioVenta = txtPrecioVenta.Text
                    tablaDatos.pproFechaVencimiento = dtpFechaVencimiento.Value
                    tablaDatos.pproStock = txtStock.Text
                    tablaDatos.pproImagen = almacenamientoImagen.GetBuffer
                    If funcion.editar(tablaDatos) Then
                        MessageBox.Show("Producto fue actualizado correctamente",
                                        "Actualizando registro", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("Producto no fue actualizado correctamente",
                                        "Actualizando registro", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
                    End If
                    mostrar()
                    limpiar()
                Catch evento As Exception
                    MsgBox(evento.Message)
                End Try
            Else
                MessageBox.Show("Cancelando actualizado por el usuario",
                                "Actualizando registro", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Falta informacion para almacenar",
                            "Actualizando registro", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cbxEliminar_CheckedChanged(sender As Object, e As EventArgs) Handles cbxEliminar.CheckedChanged
        If cbxEliminar.CheckState = CheckState.Checked Then
            dataListado.Columns.Item("eliminar").Visible = True
            btnEliminar.Visible = True
            btnInsertar.Visible = False
        Else
            dataListado.Columns.Item("eliminar").Visible = False
            btnEliminar.Visible = False
            btnInsertar.Visible = True
        End If
    End Sub

    Private Sub dataListado_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellContentClick
        If e.ColumnIndex = Me.dataListado.Columns.Item("eliminar").Index Then
            Dim chkcell As DataGridViewCheckBoxCell = Me.dataListado.Rows(e.RowIndex).Cells("eliminar")
            chkcell.Value = Not chkcell.Value
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim resultado As DialogResult
        resultado = MessageBox.Show("Desea eliminar los datos",
                        "Eliminando registro", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question)
        If resultado = Windows.Forms.DialogResult.OK Then
            Try
                For Each row As DataGridViewRow In dataListado.Rows
                    Dim lineaMarcada As Boolean = Convert.ToBoolean(row.Cells("eliminar").Value)
                    If lineaMarcada Then
                        Dim llavePrimaria As Integer = Convert.ToInt32(row.Cells("cliCodigo").Value)
                        Dim tablaDatos As New eproducto
                        Dim funcion As New fproducto
                        tablaDatos.pproCodigo = llavePrimaria
                        If funcion.eliminar(tablaDatos) Then
                            MessageBox.Show("Producto fue eliminado correctamente",
                                            "Eliminando registro", MessageBoxButtons.OK,
                                            MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("Producto no fue eliminado correctamente",
                                            "Elimando registro", MessageBoxButtons.OK,
                                            MessageBoxIcon.Error)
                        End If
                    End If
                Next
                Call mostrar()
                Call limpiar()
            Catch evento As Exception
                MsgBox(evento.Message)
            End Try
        Else
            MessageBox.Show("Cancelando eliminacion por el usuario",
                         "Eliminando registro", MessageBoxButtons.OK,
                         MessageBoxIcon.Information)
            mostrar()
            limpiar()
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        mostrar()
        limpiar()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub btnBuscarCategoria_Click(sender As Object, e As EventArgs) Handles btnBuscarCategoria.Click
        frmcategoria.bandera = True
        frmcategoria.ShowDialog()
        txtCatCodigo.Text = frmcategoria.txtCodigo.Text
        txtCategoria.Text = frmcategoria.txtDescripcion.Text
    End Sub

    Private Sub btnAgregarImagen_Click(sender As Object, e As EventArgs) Handles btnAgregarImagen.Click
        If ofdAbrir.ShowDialog = DialogResult.OK Then
            picImagen.BackgroundImage = Nothing
            picImagen.Image = New Bitmap(ofdAbrir.FileName)
            picImagen.SizeMode = PictureBoxSizeMode.StretchImage

        End If
    End Sub

    Private Sub btnQuitarImagen_Click(sender As Object, e As EventArgs) Handles btnQuitarImagen.Click
        picImagen.Image = Nothing
        picImagen.BackgroundImage = Ventas.My.Resources.Resources.noImagen
        picImagen.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub
End Class
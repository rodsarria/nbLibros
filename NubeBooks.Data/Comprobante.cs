//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NubeBooks.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comprobante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comprobante()
        {
            this.AreaPorComprobante = new HashSet<AreaPorComprobante>();
            this.Movimiento = new HashSet<Movimiento>();
        }
    
        public int IdComprobante { get; set; }
        public int IdTipoComprobante { get; set; }
        public int IdTipoDocumento { get; set; }
        public int IdEntidadResponsable { get; set; }
        public int IdMoneda { get; set; }
        public int IdEmpresa { get; set; }
        public string NroDocumento { get; set; }
        public decimal Monto { get; set; }
        public decimal MontoSinIGV { get; set; }
        public decimal TipoCambio { get; set; }
        public Nullable<int> IdArea { get; set; }
        public Nullable<int> IdResponsable { get; set; }
        public Nullable<int> IdCategoria { get; set; }
        public Nullable<int> IdProyecto { get; set; }
        public System.DateTime FechaEmision { get; set; }
        public Nullable<System.DateTime> FechaConclusion { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
        public bool Ejecutado { get; set; }
        public Nullable<int> IdHonorario { get; set; }
        public int UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaPago { get; set; }
        public Nullable<int> IdLogUsuario { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AreaPorComprobante> AreaPorComprobante { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual EntidadResponsable EntidadResponsable { get; set; }
        public virtual Honorario Honorario { get; set; }
        public virtual LogUsuario LogUsuario { get; set; }
        public virtual Moneda Moneda { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual TipoComprobante TipoComprobante { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual Usuario Usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movimiento> Movimiento { get; set; }
    }
}

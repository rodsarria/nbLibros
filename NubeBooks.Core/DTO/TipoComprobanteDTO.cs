﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class TipoComprobanteDTO
    {
        public int IdTipoComprobante { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}

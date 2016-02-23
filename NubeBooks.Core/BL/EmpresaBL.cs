﻿using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class EmpresaBL : Base
    {
        public List<EmpresaDTO> getEmpresas()
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Descripcion = x.Descripcion,
                    TipoCambio = x.TipoCambio,
                    IdPeriodo = x.IdPeriodo
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public List<EmpresaDTO> getEmpresasActivas()
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Where(x => x.Estado).Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Descripcion = x.Descripcion,
                    TipoCambio = x.TipoCambio,
                    IdPeriodo = x.IdPeriodo,
                    IdMoneda = x.IdMoneda,
                    SimboloMoneda = x.Moneda.Simbolo,
                    TotalSoles = x.TotalSoles,
                    TotalDolares = x.TotalDolares,
                    TotalSolesOld = x.TotalSolesOld,
                    TotalDolaresOld = x.TotalDolaresOld,
                    FechaConciliacion = x.FechaConciliacion
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public EmpresaDTO getEmpresa(int id)
        {
            using (var context = getContext())
            {
                var result = context.Empresa.Where(x => x.IdEmpresa == id)
                    .Select(r => new EmpresaDTO
                    {
                        IdEmpresa = r.IdEmpresa,
                        Nombre = r.Nombre,
                        Estado = r.Estado,
                        Descripcion = r.Descripcion,
                        TipoCambio = r.TipoCambio,
                        IdPeriodo = r.IdPeriodo,
                        IdMoneda = r.IdMoneda,
                        SimboloMoneda = r.Moneda.Simbolo,
                        TotalSoles = r.TotalSoles,
                        TotalDolares = r.TotalDolares,
                        TotalSolesOld = r.TotalSolesOld,
                        TotalDolaresOld = r.TotalDolaresOld,
                        FechaConciliacion = r.FechaConciliacion
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    Empresa nuevo = new Empresa();
                    nuevo.Nombre = Empresa.Nombre;
                    nuevo.Estado = true;
                    nuevo.Descripcion = Empresa.Descripcion;
                    nuevo.TipoCambio = Empresa.TipoCambio == 0 ? 1 : Empresa.TipoCambio;
                    nuevo.IdPeriodo = Empresa.IdPeriodo;
                    context.Empresa.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Empresa.Where(x => x.IdEmpresa == Empresa.IdEmpresa).SingleOrDefault();
                    row.Nombre = Empresa.Nombre;
                    row.Estado = Empresa.Estado;
                    row.Descripcion = Empresa.Descripcion;
                    row.TipoCambio = Empresa.TipoCambio;
                    row.IdPeriodo = Empresa.IdPeriodo;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool updateTipoCambio(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Empresa.Where(x => x.IdEmpresa == Empresa.IdEmpresa).SingleOrDefault();
                    row.TipoCambio = Empresa.TipoCambio;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool updatePeriodo(EmpresaDTO Empresa)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Empresa.Where(x => x.IdEmpresa == Empresa.IdEmpresa).SingleOrDefault();
                    row.IdPeriodo = Empresa.IdPeriodo;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool updateMontosSolesDolares(int id)
        {
            using (var context = getContext())
            {
                try
                {
                    context.SP_ActualizarMontos_Empresa(id);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<EmpresaDTO> getEmpresasViewBag()
        {
            using (var context = getContext())
            {
                var lista = context.Empresa.Where(x => x.Estado).Select(x => new EmpresaDTO
                {
                    IdEmpresa = x.IdEmpresa,
                    Nombre = x.Nombre
                }).ToList();
                return lista;
            }
        }

        public List<LiquidezDTO> getLiquidezEnEmpresaPorMoneda(int idEmpresa, int moneda)
        {
            using (var context = getContext())
            {
                DateTime today = new DateTime();
                today = DateTime.Now;
                //Ultimo dia del mes y dia inicial del mes actual
                DateTime firstDayMonth = new DateTime(today.Year, today.Month, 1);
                DateTime lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);
                //Ultimo dia inicial de hace un año atras (YA - Year Ago)
                DateTime firstDayMonthYA = new DateTime(today.Year - 1, today.Month + 1, 1);

                //Conseguir todos movimientos desde el mes actual hasta 11 meses atras (12 meses)
                var lista = from mov in context.Movimiento
                            join lib in context.CuentaBancaria on mov.IdCuentaBancaria equals lib.IdCuentaBancaria
                            join fmov in context.FormaMovimiento on mov.IdFormaMovimiento equals fmov.IdFormaMovimiento
                            //join tmov in context.TipoMovimiento on mov.IdTipoDocumento equals tmov.IdTipoMovimiento
                            where lib.IdEmpresa == idEmpresa && lib.IdMoneda == moneda && mov.Estado == true && (mov.FechaCreacion <= lastDayMonth && mov.FechaCreacion >= firstDayMonthYA)
                            select new MovimientoDTO
                            {
                                IdMovimiento = mov.IdMovimiento,
                                IdTipoMovimiento = fmov.IdTipoMovimiento,
                                Fecha = mov.Fecha,
                                FechaCreacion = mov.FechaCreacion,
                                Monto = mov.Monto,
                                MontoSinIGV = mov.MontoSinIGV,
                                TipoCambio = mov.TipoCambio
                            };

                List<MovimientoDTO> listMovs = lista.OrderBy(x => x.FechaCreacion).ToList();

                //Obtener Liquidez por cada mes segun la moneda que corresponda
                List<LiquidezDTO> lstLiq = new List<LiquidezDTO>();

                for (int i = 0; i < 12; i++)
                {
                    LiquidezDTO nuevo = new LiquidezDTO();
                    nuevo.Mes = (today.Month - i) > 0 ? (today.Month - i) : (today.Month + 12 - i);
                    nuevo.Monto = listMovs.Where(x => x.IdTipoMovimiento == 1 && x.FechaCreacion.Month == nuevo.Mes).Sum(x => x.Monto) - listMovs.Where(x => x.IdTipoMovimiento == 2 && x.FechaCreacion.Month == nuevo.Mes).Sum(x => x.Monto);
                    lstLiq.Add(nuevo);
                }
                
                return lstLiq;
            }
        }

        public List<LiquidezDTO> getRentabilidadEnEmpresaSegunMoneda(int idEmpresa, int Moneda)
        {
            using (var context = getContext())
            {
                DateTime today = new DateTime();
                today = DateTime.Now;
                //Ultimo dia del mes y dia inicial del mes actual
                DateTime firstDayMonth = new DateTime(today.Year, today.Month, 1);
                DateTime lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);
                //Ultimo dia inicial de hace un año atras (YA - Year Ago)
                DateTime firstDayMonthYA = new DateTime(today.Year - 1, today.Month + 1, 1);

                List<ComprobanteDTO> lista = (from cmp in context.Comprobante
                            where cmp.IdEmpresa == idEmpresa && cmp.Estado == true && (cmp.FechaEmision <= lastDayMonth && cmp.FechaEmision >= firstDayMonthYA)
                            select new ComprobanteDTO
                            {
                                IdComprobante = cmp.IdComprobante,
                                IdTipoComprobante = cmp.IdTipoComprobante,
                                FechaEmision = cmp.FechaEmision,
                                Monto = cmp.Monto,
                                MontoSinIGV = cmp.MontoSinIGV,
                                TipoCambio = cmp.TipoCambio,
                                IdMoneda = cmp.IdMoneda
                            }).ToList();

                List<LiquidezDTO> lstRent = new List<LiquidezDTO>();

                switch (Moneda)
                {
                    case 1:
                        for (int i = 0; i < 12; i++)
                        {
                            LiquidezDTO nuevo = new LiquidezDTO();
                            nuevo.Mes = (today.Month - i) > 0 ? (today.Month - i) : (today.Month + 12 - i);
                            Decimal Soles = 0, Dolares = 0;
                            //Total de Soles + Total de Dolares a ==> SOLES
                            Soles = lista.Where(x => x.IdTipoComprobante == 1 && x.IdMoneda == 1 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto) - lista.Where(x => x.IdTipoComprobante == 2 && x.IdMoneda == 1 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto);
                            Dolares = lista.Where(x => x.IdTipoComprobante == 1 && x.IdMoneda == 2 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto * x.TipoCambio) - lista.Where(x => x.IdTipoComprobante == 2 && x.IdMoneda == 2 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto * x.TipoCambio);

                            nuevo.Monto = Soles + Dolares;
                            lstRent.Add(nuevo);
                        }
                        break;
                    default:
                        for (int i = 0; i < 12; i++)
                        {
                            LiquidezDTO nuevo = new LiquidezDTO();
                            nuevo.Mes = (today.Month - i) > 0 ? (today.Month - i) : (today.Month + 12 - i);
                            Decimal Soles = 0, Dolares = 0;
                            //Total de Soles + Total de Dolares a ==> SOLES
                            Soles = lista.Where(x => x.IdTipoComprobante == 1 && x.IdMoneda == 1 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto / (x.TipoCambio != 0 ? x.TipoCambio : 1)) - lista.Where(x => x.IdTipoComprobante == 2 && x.IdMoneda == 1 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto / (x.TipoCambio != 0 ? x.TipoCambio : 1));
                            Dolares = lista.Where(x => x.IdTipoComprobante == 1 && x.IdMoneda == 2 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto) - lista.Where(x => x.IdTipoComprobante == 2 && x.IdMoneda == 2 && x.FechaEmision.Month == nuevo.Mes).Sum(x => x.Monto);

                            nuevo.Monto = Soles + Dolares;
                            lstRent.Add(nuevo);
                        }
                        break;
                }

                return lstRent;
            }
        }

        public Decimal getEjecucionDePresupuestoEnEmpresa(int idEmpresa, int idMoneda, int idPeriodo, int tipo)
        {
            using (var context = getContext())
            {
                var periodo = context.Periodo.Where(x => x.IdPeriodo == idPeriodo).SingleOrDefault();
                //INGRESOS Y EGRESOS
                var cadena = tipo == 1 ? "INGRESOS" : "EGRESOS";
                var prep = (from cat in context.Categoria
                           join cp in context.CategoriaPorPeriodo on cat.IdCategoria equals cp.IdCategoria
                           where cat.IdEmpresa == idEmpresa && cp.IdPeriodo == idPeriodo && cat.Nombre == cadena
                           select new CategoriaPorPeriodoDTO
                           {
                               IdCategoria = cp.IdCategoria,
                               IdPeriodo = cp.IdPeriodo,
                               Monto = cp.Monto
                           }).SingleOrDefault();

                if (prep == null)
                {
                    return 0;
                }

                Decimal presupuesto = prep.Monto;

                var lstCmp = context.Comprobante.Where(x => x.IdEmpresa == idEmpresa && x.Estado == true && (x.FechaEmision <= periodo.FechaFin && x.FechaEmision >= periodo.FechaInicio) && x.IdTipoComprobante == tipo).ToList();

                Decimal Soles = 0, Dolares = 0;

                switch (idMoneda)
                {
                    case 1:
                        Soles = lstCmp.Where(x => x.IdMoneda == 1).Sum(x => x.Monto);
                        Dolares = lstCmp.Where(x => x.IdMoneda == 2).Sum(x => x.Monto * x.TipoCambio);
                        break;
                    default:
                        Soles = lstCmp.Where(x => x.IdMoneda == 1).Sum(x => x.Monto / (x.TipoCambio != 0 ? x.TipoCambio : 1));
                        Dolares = lstCmp.Where(x => x.IdMoneda == 2).Sum(x => x.Monto);
                        break;
                }

                presupuesto = (Soles + Dolares) / presupuesto;

                return presupuesto;
            }
        }
        public List<EntidadResponsableR_DTO> getTop5Clientes(int idEmpresa, int idPeriodo)
        {
            using (var context = getContext())
            {
                var periodo = context.Periodo.Where(x => x.IdPeriodo == idPeriodo).SingleOrDefault();

                if (periodo == null) { return new List<EntidadResponsableR_DTO>(); }

                var result = context.SP_Rep_FacturacionPorCliente(idEmpresa, periodo.FechaInicio, periodo.FechaFin).Select(x => new EntidadResponsableR_DTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    Monto = x.Monto.GetValueOrDefault()
                }).OrderByDescending(x => x.Monto).ToList();

                if (result == null) { return new List<EntidadResponsableR_DTO>(); }

                Decimal montoTotal = result.Sum(x => x.Monto);

                List<EntidadResponsableR_DTO> lista = result.Take(5).Select(x => new EntidadResponsableR_DTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    Monto = x.Monto,
                    Porcentaje = montoTotal != 0 ? (x.Monto / montoTotal) : 0
                }).ToList();

                return lista;
            }
        }
        public List<EntidadResponsableR_DTO> getTop5Proveedores(int idEmpresa, int idPeriodo)
        {
            using (var context = getContext())
            {
                var periodo = context.Periodo.Where(x => x.IdPeriodo == idPeriodo).SingleOrDefault();

                if (periodo == null) { return new List<EntidadResponsableR_DTO>(); }

                var result = context.SP_Rep_GastosPorProveedor(idEmpresa, periodo.FechaInicio, periodo.FechaFin).Select(x => new EntidadResponsableR_DTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    Monto = x.Monto.GetValueOrDefault()
                }).OrderByDescending(x => x.Monto).ToList();

                if (result == null) { return new List<EntidadResponsableR_DTO>(); }
                
                Decimal montoTotal = result.Sum(x => x.Monto);

                List<EntidadResponsableR_DTO> lista = result.Take(5).Select(x => new EntidadResponsableR_DTO
                {
                    IdEntidadResponsable = x.IdEntidadResponsable,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                    Tipo = x.Tipo,
                    IdEmpresa = x.IdEmpresa,
                    Monto = x.Monto,
                    Porcentaje = montoTotal != 0 ? (x.Monto / montoTotal) : 0
                }).ToList();

                return lista;
            }
        }
        public List<AreaDTO> getTopIngArea(int idEmpresa, int idPeriodo)
        {
            using (var context = getContext())
            {
                var periodo = context.Periodo.Where(x => x.IdPeriodo == idPeriodo).SingleOrDefault();

                if (periodo == null) { return new List<AreaDTO>(); }

                var result = context.SP_Rep_IngresosEgresosPorAreas(idEmpresa, periodo.FechaInicio, periodo.FechaFin).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    Ingresos = x.Ingreso.GetValueOrDefault(),
                    Egresos = x.Egreso.GetValueOrDefault()
                }).OrderByDescending(x => x.Ingresos).ToList();

                if (result == null) { return new List<AreaDTO>(); }

                Decimal montoTotal = result.Sum(x => x.Ingresos);

                List<AreaDTO> lista = result.Take(5).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    Ingresos = x.Ingresos,
                    Egresos = x.Egresos,
                    Porcentaje = montoTotal != 0 ? (x.Ingresos / montoTotal) : 0
                }).ToList();

                AreaDTO Otros = new AreaDTO() { Nombre = "Otros", Estado = true, IdEmpresa = 1 };
                Otros.Ingresos = result.Skip(5).Sum(x => x.Ingresos);
                Otros.Egresos = result.Skip(5).Sum(x => x.Egresos);
                Otros.Porcentaje = montoTotal != 0 ? (Otros.Ingresos / montoTotal) : 0;

                lista.Add(Otros);

                return lista;
            }
        }
        public List<AreaDTO> getTopEgrArea(int idEmpresa, int idPeriodo)
        {
            using (var context = getContext())
            {
                var periodo = context.Periodo.Where(x => x.IdPeriodo == idPeriodo).SingleOrDefault();

                if (periodo == null) { return new List<AreaDTO>(); }

                var result = context.SP_Rep_IngresosEgresosPorAreas(idEmpresa, periodo.FechaInicio, periodo.FechaFin).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    Ingresos = x.Ingreso.GetValueOrDefault(),
                    Egresos = x.Egreso.GetValueOrDefault()
                }).OrderBy(x => x.Egresos).ToList();

                if (result == null) { return new List<AreaDTO>(); }

                Decimal montoTotal = result.Sum(x => x.Egresos);

                List<AreaDTO> lista = result.Take(5).Select(x => new AreaDTO
                {
                    IdArea = x.IdArea,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    Ingresos = x.Ingresos,
                    Egresos = x.Egresos,
                    Porcentaje = montoTotal != 0 ? (x.Egresos / montoTotal) : 0
                }).ToList();

                AreaDTO Otros = new AreaDTO() { Nombre = "Otros", Estado = true, IdEmpresa = 1 };
                Otros.Ingresos = result.Skip(5).Sum(x => x.Ingresos);
                Otros.Egresos = result.Skip(5).Sum(x => x.Egresos);
                Otros.Porcentaje = montoTotal != 0 ? (Otros.Egresos / montoTotal) : 0;

                lista.Add(Otros);

                return lista;
            }
        }

        public List<int> getContadorDeMovimientos(int idEmpresa)
        {
            using (var context = getContext())
            {
                DateTime today = new DateTime();
                today = DateTime.Now;
                //Ultimo dia del mes y dia inicial del mes actual
                DateTime firstDayMonth = new DateTime(today.Year, today.Month, 1);
                DateTime lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);
                //Ultimo dia inicial de hace un año atras (YA - Year Ago)
                DateTime firstDayMonthYA = new DateTime(today.Year - 1, today.Month + 1, 1);

                var lista = from mov in context.Movimiento
                            join lib in context.CuentaBancaria on mov.IdCuentaBancaria equals lib.IdCuentaBancaria
                            where lib.IdEmpresa == idEmpresa && mov.Estado == true && (mov.FechaCreacion <= lastDayMonth && mov.FechaCreacion >= firstDayMonthYA)
                            select new MovimientoDTO
                            {
                                IdMovimiento = mov.IdMovimiento,
                                Fecha = mov.Fecha,
                                FechaCreacion = mov.FechaCreacion,
                                Monto = mov.Monto,
                                MontoSinIGV = mov.MontoSinIGV,
                                TipoCambio = mov.TipoCambio
                            };

                List<MovimientoDTO> listMovs = lista.OrderBy(x => x.FechaCreacion).ToList();

                //Obtener Liquidez por cada mes segun la moneda que corresponda
                List<int> lstCont = new List<int>();
                int mes = today.Month;
                for (int i = 0; i < 12; i++)
                {
                    mes = (today.Month - i) > 0 ? (today.Month - i) : (today.Month + 12 - i);
                    lstCont.Add(listMovs.Where(x => x.FechaCreacion.Month == mes).Count());
                }
                return lstCont;
            }
        }

        public List<int> getContadorDeComprobantes(int idEmpresa)
        {
            using (var context = getContext())
            {
                DateTime today = new DateTime();
                today = DateTime.Now;
                //Ultimo dia del mes y dia inicial del mes actual
                DateTime firstDayMonth = new DateTime(today.Year, today.Month, 1);
                DateTime lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);
                //Ultimo dia inicial de hace un año atras (YA - Year Ago)
                DateTime firstDayMonthYA = new DateTime(today.Year - 1, today.Month + 1, 1);

                List<ComprobanteDTO> lista = (from cmp in context.Comprobante
                                              where cmp.IdEmpresa == idEmpresa && cmp.Estado == true && (cmp.FechaEmision <= lastDayMonth && cmp.FechaEmision >= firstDayMonthYA)
                                              select new ComprobanteDTO
                                              {
                                                  IdComprobante = cmp.IdComprobante,
                                                  IdTipoComprobante = cmp.IdTipoComprobante,
                                                  FechaEmision = cmp.FechaEmision,
                                                  Monto = cmp.Monto,
                                                  MontoSinIGV = cmp.MontoSinIGV,
                                                  TipoCambio = cmp.TipoCambio,
                                                  IdMoneda = cmp.IdMoneda
                                              }).OrderBy(x => x.FechaEmision).ToList();

                //Obtener Liquidez por cada mes segun la moneda que corresponda
                List<int> lstCont = new List<int>();
                int mes = today.Month;
                for (int i = 0; i < 12; i++)
                {
                    mes = (today.Month - i) > 0 ? (today.Month - i) : (today.Month + 12 - i);
                    lstCont.Add(lista.Where(x => x.FechaEmision.Month == mes).Count());
                }

                return lstCont;
            }
        }
    }
}

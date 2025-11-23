using AegisTasks.BLL.DataAccess;
using AegisTasks.Core.DTO;
using AegisTasks.UI.Forms;
using AegisTasks.UI.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AegisTasks.UI.Presenters
{
    public class HistoryViewerPresenter : AegisFormPresenterBase<HistoryViewer>
    {
        public HistoryViewerPresenter(HistoryViewer view) : base(view)
        {
        }

        public override void Initialize()
        {
            HistoryViewer view = this._View;

            view.Text = Texts.History;

            // Obtener historial del usuario actual
            List<ExecutionHistoryLineDTO> history = ExecutionHistoryDataAccessBLL.GetExecutionsByUser(SessionManager.CurrentUser.Username);

            // Limpiar columnas y filas existentes
            view.HistoryViewerTable.Columns.Clear();
            view.HistoryViewerTable.Rows.Clear();

            // Configurar columnas usando constantes del DTO
            view.HistoryViewerTable.Columns.Add(ExecutionHistoryLineDTO.FIELD_ID, Texts.Id);
            view.HistoryViewerTable.Columns.Add(ExecutionHistoryLineDTO.FIELD_WORKFLOWID, Texts.TaskPlanId);
            view.HistoryViewerTable.Columns.Add(ExecutionHistoryLineDTO.FIELD_WORKFLOWNAME, Texts.TaskPlanName);
            view.HistoryViewerTable.Columns.Add(ExecutionHistoryLineDTO.FIELD_STARTDATE, Texts.StartDate);
            view.HistoryViewerTable.Columns.Add(ExecutionHistoryLineDTO.FIELD_ENDDATE, Texts.EndDate);
            view.HistoryViewerTable.Columns.Add(ExecutionHistoryLineDTO.FIELD_SUCCESS, Texts.Sucessful);

            // Hacer que todas las columnas llenen proporcionalmente el espacio disponible
            foreach (DataGridViewColumn col in view.HistoryViewerTable.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Llenar filas
            foreach (var exec in history)
            {
                view.HistoryViewerTable.Rows.Add(
                    exec.Id,
                    exec.WorkflowId,
                    exec.WorkflowName,
                    exec.StartDate?.ToString("g") ?? "",
                    exec.EndDate?.ToString("g") ?? "",
                    exec.Success
                );
            }
        }

        protected override bool isLoadAllowed()
        {
            return this.isLogged();
        }
    }
}

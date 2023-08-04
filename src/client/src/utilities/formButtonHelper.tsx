import { CreateFormButtons, EditFormButtons } from "../components/FormButtons";
import { store } from "../store";
import { addAlert } from "../store/AlertState";
import { createFormErrorAlert } from "./formHelper";

const formButtonHelper = ({
  domain,
  formType,
  hasErrors,
  setErrors,
  formValues,
  formActions,
  isDeleted,
} : any) => {
  let buttons = null;
  if (formType === "create") {
    buttons = (
      <CreateFormButtons
        domain={domain}
        handleSave={async (event: any) => {
          event.preventDefault();
          const errors = hasErrors(formValues);
          if (errors) {
            store.dispatch(addAlert(createFormErrorAlert()));
            setErrors(errors);
          } else {
            await formActions.addItem(formValues);
          }
        }}
      />
    );
  } else if (formType === "edit") {
    buttons = (
      <EditFormButtons
        domain={domain}
        handleSave={async (event: any) => {
          event.preventDefault();
          const errors = hasErrors(formValues);
          if (errors) {
            store.dispatch(addAlert(createFormErrorAlert()));
            setErrors(errors);
          } else {
            await formActions.updateItem(formValues);
          }
        }}
        handleDelete={async () =>
          isDeleted
            ? await formActions.deleteItem()
            : await formActions.updateItem({
                ...formValues,
                isDeleted: true,
              })
        }
      />
    );
  }
  return buttons;
};

export default formButtonHelper;

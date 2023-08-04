import { store } from "../store";
import { IAlert, addAlert } from "../store/AlertState";
import { DELETE, POST, PUT } from "./httpRequest";
import createUniqueKey from "./uniqueKey";

const formHelper = ({ domain, id, navigate }: any) => {
  return {
    addItem: async (data: any) => {
      try {
        await POST({ endpoint: `${domain}`, data });
        navigate(`/admin/${domain}`);
      } catch (error: any) {
        store.dispatch(
          addAlert({
            id: createUniqueKey(10),
            type: "danger",
            message: error.message,
          } as IAlert)
        );
      }
    },

    updateItem: async (data: any) => {
      try {
        await PUT({ endpoint: `${domain}/${id}`, data });
        navigate(`/admin/${domain}`);
      } catch (error: any) {
        store.dispatch(
          addAlert({
            id: createUniqueKey(10),
            type: "danger",
            message: error.message,
          } as IAlert)
        );
      }
    },

    deleteItem: async () => {
      try {
        await DELETE({ endpoint: `${domain}/${id}` });
        navigate(`/admin/${domain}`);
      } catch (error: any) {
        store.dispatch(
          addAlert({
            id: createUniqueKey(10),
            type: "danger",
            message: error.message,
          } as IAlert)
        );
      }
    },
  };
};

export const createFormErrorAlert = () => {
  return {
    id: createUniqueKey(10),
    type: "warning",
    message: "Invalid form, please correct the highlighted fields.",
  } as IAlert;
};

export default formHelper;

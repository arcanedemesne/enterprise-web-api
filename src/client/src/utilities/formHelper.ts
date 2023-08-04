import { IAlert } from "../store/AlertState";
import { DELETE, POST, PUT } from "./httpRequest";
import createUniqueKey from "./uniqueKey";

const formHelper = ({ domain, id, navigate }: any) => {
  return {
    addItem: async (data: any) => {
      await POST({ endpoint: `${domain}`, data });
      navigate(`/admin/${domain}`);
    },

    updateItem: async (data: any) => {
      await PUT({ endpoint: `${domain}/${id}`, data });
      navigate(`/admin/${domain}`);
    },

    deleteItem: async () => {
      await DELETE({ endpoint: `${domain}/${id}` });
      navigate(`/admin/${domain}`);
    },
  };
};

export const createFormErrorAlert = () => {
  return {
    id: createUniqueKey(10),
    type: "warning",
    message: "Invalid form, please correct the highlighted fields.",
  } as IAlert;
}

export default formHelper;

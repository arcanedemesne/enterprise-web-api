import { DELETE, POST, PUT } from "./httpRequest";

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

export default formHelper;

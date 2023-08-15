import { store } from "../store";

const userHelper = {
  mapUserToKeycloakId: (kcId: string) =>
    store
      .getState()
      .userState.allUsers.find((u) => u.keycloakUniqueIdentifier === kcId),
};

export default userHelper;

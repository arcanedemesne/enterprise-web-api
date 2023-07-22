import { redirect } from "react-router-dom";
import { deleteContact } from "../contacts";

export function action({ params }: any) {
  deleteContact(params.contactId);
  return redirect("/");
}
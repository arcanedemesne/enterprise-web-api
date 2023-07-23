import { useLoaderData } from "react-router-dom";
import { isUserLoggedIn, getMetadata, logout } from "../auth/user";

export function loader() {
  const isloggedIn = isUserLoggedIn();
  return { isloggedIn };
}

const Dashboard = () => {
  const { isloggedIn } = useLoaderData() as { isloggedIn: boolean };
  if (!isloggedIn) window.location.href = "/login";

  return (
    <div>
      <h1>Dashboard</h1>
      Welcome, <b>{getMetadata()?.full_name}</b>
      <button onClick={logout}>Log Out</button>
    </div>
  );
};

export default Dashboard;
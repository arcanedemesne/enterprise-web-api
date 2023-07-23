import { getMetadata, logout } from "../auth/user";

const Dashboard = () => {
  return (
    <div>
      <h1>Dashboard</h1>
      Welcome, <b>{getMetadata()?.full_name}</b>
      <button onClick={logout}>Log Out</button>
    </div>
  );
};

export default Dashboard;
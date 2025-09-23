import Navbar from "react-bootstrap/Navbar";
import { useAdminPageEnvironmentContext } from "../../providers/AdminPageEnvironmentContextProvider";

export function AdminPageHeader() {
  const environmentContext = useAdminPageEnvironmentContext();

  return (
    <Navbar bg="black" style={{ color: "white" }}>
      <Navbar.Brand href="" style={{ color: "white", marginLeft: "16px" }}>
        Unified Contacts Portal
      </Navbar.Brand>
      {environmentContext.versionInfo?.version}
    </Navbar>
  );
}

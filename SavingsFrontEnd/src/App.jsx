import "./App.css";

import "@mantine/core/styles.css";
import { Savings } from "./Savings";
import { MantineProvider } from "@mantine/core";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
    <MantineProvider>
      <Router>
        <Routes>
          <Route path="/" element={<Savings />} />
        </Routes>
      </Router>
    </MantineProvider>
  );
}

export default App;

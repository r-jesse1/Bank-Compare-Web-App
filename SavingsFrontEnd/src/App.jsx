import "./App.css";

import "@mantine/core/styles.css";
import { shadcnCssVariableResolver } from "./cssVariableResolver.ts";
import { shadcnTheme } from "./theme.ts";
import { MantineProvider } from "@mantine/core";

import { Savings } from "./Savings";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
    <MantineProvider
      theme={shadcnTheme}
      cssVariablesResolver={shadcnCssVariableResolver}
    >
      <Router>
        <Routes>
          <Route path="/" element={<Savings />} />
        </Routes>
      </Router>
    </MantineProvider>
  );
}

export default App;

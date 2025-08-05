import React, { useState } from "react";
import { Button, Modal } from "@mantine/core";
import { LineChart } from "@mantine/charts";
import { useDisclosure } from "@mantine/hooks";

export function SavingsRateChartModal({ data, open, setOpen }) {
  return (
    <>
      <Modal
        opened={open}
        onClose={(_) => setOpen(false)}
        title="Savings Rate Over Time"
        size="lg"
        centered
      >
        <div>
          <LineChart
            h={300}
            data={data}
            dataKey="date"
            series={[{ name: "totalRate", color: "indigo.6" }]}
            curveType="linear"
          />
        </div>
      </Modal>

      <Button onClick={(_) => setOpen(true)}>View Chart</Button>
    </>
  );
}

import {
  Text,
  Title,
  Group,
  Grid,
  Button,
  Image,
  Paper,
  Modal,
} from "@mantine/core";
import { LineChart } from "@mantine/charts";
import { useState } from "react";

import { StackedText } from "./components/StackedText";
import { SavingsRateChartModal } from "./components/SavingsRateChartModal";

export function SavingsCard({ account, userBalance, setHistory, setOpen }) {
  let baseRate = account.baseRate;
  let bonusRate = account.bonusRate;
  let totalRate = account.totalRate;
  let introRate = account.introRate;

  let interest = 0;
  if (Number.isFinite(userBalance)) {
    const r = account.totalRate / 100;
    const n = 12; // monthly compounding
    const t = 1; // 1 year
    interest = userBalance * Math.pow(1 + r / n, n * t) - userBalance;
  }

  async function fetchHistory(url) {
    try {
      const response = await fetch(
        `http://localhost:5134/savingsrate/history?url=${url}`
      );

      if (!response.ok) {
        throw new Error(`Server error: ${response.status}`);
      }

      const data = await response.json();
      console.log(data);
      setHistory(data);
      setOpen(true);
    } catch (e) {
      console.log(e);
    }
  }

  return (
    <Paper style={{ maxWidth: 1200 }} shadow="xl" withBorder p="xl">
      <Grid align="center">
        {/* Logo and Name */}
        <Grid.Col span={2}>
          {/* <Image
            src={`https://logo.clearbit.com/${new URL(account.url).hostname}`}
            alt="Bank logo"
            width={120}
            height={120}
            fit="contain"
          /> */}
          <Image
            src={`/${account.bank}.webp`}
            alt={`${account.bank} logo`}
            width={60}
            height={60}
            fit="contain"
            data-attempt="webp"
            onError={(e) => {
              const img = e.currentTarget;
              const attempt = img.getAttribute("data-attempt");

              if (attempt === "webp") {
                img.src = `/${account.bank}.png`;
                img.setAttribute("data-attempt", "png");
              } else if (attempt === "png") {
                img.src = `/default.png`;
                img.setAttribute("data-attempt", "default");
              } else {
                // Prevent further looping
                console.warn(`Failed to load image for bank: ${account.bank}`);
                img.onerror = null;
              }
            }}
          />
        </Grid.Col>

        <Grid.Col span={6}>
          <Title order={3}>
            {account.bank} - {account.name}
          </Title>
        </Grid.Col>

        {/* Interest Breakdown */}
        <Grid.Col span={6} offset={3}>
          <Group justify="space-between" gap="xl">
            <StackedText label="Base Rate" body={`${baseRate?.toFixed(2)}%`} />
            <StackedText
              label="Bonus Rate"
              body={bonusRate === 0 ? "—" : `${bonusRate?.toFixed(2)}%`}
            />
            <StackedText
              label="Intro Rate"
              body={introRate === 0 ? "—" : `${introRate?.toFixed(2)}%`}
            />
            <StackedText
              label="Total Rate"
              body={totalRate?.toFixed(2) + "%"}
            />
            <StackedText
              label="Savings (p.a)"
              body={"$" + interest?.toFixed(2)}
            />
          </Group>
        </Grid.Col>

        {/* Description and CTA */}
        <Grid.Col span={10}>
          <Text size="sm" mt="sm">
            {account.bonusConditions ||
              account.introConditions ||
              "No bonus conditions provided."}
          </Text>
        </Grid.Col>

        {/* CTA Button */}
        <Grid.Col span={2} mt="md">
          <Button
            component="a"
            href={account.productURL || undefined}
            target="_blank"
            disabled={!account.productURL}
            onClick={(e) => {
              if (!account.productURL) {
                e.preventDefault();
              }
            }}
          >
            Visit Site
          </Button>
        </Grid.Col>

        <Grid.Col span={2} mt="md">
          <Button onClick={(_) => fetchHistory(account.url)}>Chart</Button>
        </Grid.Col>
      </Grid>
    </Paper>
  );
}

import {
  Card,
  Text,
  Badge,
  Stack,
  Container,
  Anchor,
  Title,
  Group,
  Grid,
  Button,
  Image,
  Checkbox,
  SegmentedControl,
  Paper,
} from "@mantine/core";
import {
  MantineProvider,
  useMantineColorScheme,
  ActionIcon,
  useComputedColorScheme,
} from "@mantine/core";
import { StackedText } from "./components/StackedText";
import { IconSun, IconMoon } from "@tabler/icons-react";

import { useState, useEffect } from "react";

export function SavingsCard({ account, userBalance }) {
  let baseRate = account.baseRate;
  let bonusRate = account.bonusRate;
  let totalRate = account.totalRate;

  let interest = 0;
  if (Number.isFinite(userBalance)) {
    interest = userBalance * account.totalRate;
  }
  console.log(userBalance);
  return (
    <Paper style={{ maxWidth: 1200 }} shadow="xl" withBorder p="xl">
      <Grid align="center">
        {/* Logo and Name */}
        <Grid.Col span={2}>
          <Image
            src={`https://logo.clearbit.com/${new URL(account.url).hostname}`}
            alt="Bank logo"
            width={120}
            height={120}
            fit="contain"
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
            <StackedText label="Base Rate" body={baseRate?.toFixed(2) + "%"} />
            <StackedText
              label="Bonus Rate"
              body={bonusRate?.toFixed(2) + "%"}
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
            {account.bonusConditions || "No bonus conditions provided."}
          </Text>
        </Grid.Col>

        {/* CTA Button */}
        <Grid.Col span={2} mt="md">
          <Button component="a" href={account.url} target="_blank">
            Visit Site
          </Button>
        </Grid.Col>
      </Grid>
    </Paper>
  );
}

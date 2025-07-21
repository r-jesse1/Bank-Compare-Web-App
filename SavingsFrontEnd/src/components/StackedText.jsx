import { Box, Text, Grid, Center } from "@mantine/core";

export function StackedText({ label, body, alignment = "center" }) {
  return (
    <div>
      <Text size="lg" c="dimmed" style={{ marginBottom: -5 }} ta={alignment}>
        {label}
      </Text>
      <Text size="xl" fw={500} ta={alignment}>
        {body}
      </Text>
    </div>
  );
}

// <div>
//   <Text size="lg" fw={500} style={{ marginBottom: -5 }} ta={alignment}>
//     {body}
//   </Text>
//   <Text size="sm" c="dimmed" ta={alignment}>
//     {label}
//   </Text>
// </div>;

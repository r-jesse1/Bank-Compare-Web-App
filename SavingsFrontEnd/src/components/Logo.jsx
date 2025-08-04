import { useEffect, useState, useRef } from "react";
import { motion, animate } from "motion/react";
import { Text, Box, Stack, rem, Title, Badge } from "@mantine/core";

const MotionBadge = motion(Badge);

export function Logo() {
  const badgeRef = useRef(null);

  useEffect(() => {
    let controls;

    const startRocking = () => {
      // Damped oscillation keyframes (mimics momentum loss)
      controls = animate(
        badgeRef.current,
        { rotate: [0, 10, -8, 6, -4, 2, 0] },
        {
          duration: 2,
          easing: "ease-in-out",
        }
      );
    };

    startRocking();
    const interval = setInterval(startRocking, 5000);

    return () => {
      clearInterval(interval);
      controls?.cancel();
    };
  }, []);

  return (
    <div
      style={{
        padding: "50px",
      }}
    >
      <div style={{ position: "relative", display: "inline-block" }}>
        <Title style={{ fontSize: "48px", fontWeight: "bold" }}>
          BankCompare
        </Title>
        <MotionBadge
          ref={badgeRef}
          size="lg"
          style={{
            position: "absolute",
            top: "-20px",
            right: "-40px",
            transformOrigin: "center",
          }}
        >
          Aus
        </MotionBadge>
      </div>
      <Text style={{ fontSize: "25px" }} mt={20} c="dimmed">
        Savings Accounts
      </Text>
    </div>
  );
}

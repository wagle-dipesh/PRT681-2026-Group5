import "@progress/kendo-theme-default/dist/all.css";
import "./globals.css";

export const metadata = {
  title: "Tasks Management Portal",
  description: "Next.js + KendoReact admin portal for the Tasks API",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}

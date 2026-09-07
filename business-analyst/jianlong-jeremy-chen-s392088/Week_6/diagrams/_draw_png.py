"""Draw Week 6 BA diagrams as PNG with matplotlib (no mermaid renderer required)."""
from __future__ import annotations

from pathlib import Path

import matplotlib.pyplot as plt
from matplotlib.patches import Circle, Ellipse, FancyArrowPatch, FancyBboxPatch, Polygon, Rectangle
from matplotlib.patches import Arc

DIR = Path(__file__).resolve().parent

NAVY = "#1B4F72"
INK = "#1C2833"
TEAL = "#148F77"
GOLD = "#B7950B"
RED = "#922B21"
GREY = "#566573"

FILL_ACT = "#D6EAF8"
FILL_DEC = "#FCF3CF"
FILL_OK = "#D5F5E3"
FILL_BAD = "#FADBD8"
FILL_START = "#AED6F1"
FILL_STORE = "#E8DAEF"
FILL_ENT = "#FDEBD0"
FILL_STATE = "#D4E6F1"


def new_fig(w, h):
    fig, ax = plt.subplots(figsize=(w, h), dpi=160)
    ax.set_xlim(0, w)
    ax.set_ylim(0, h)
    ax.set_aspect("equal")
    ax.axis("off")
    fig.patch.set_facecolor("white")
    ax.set_facecolor("white")
    return fig, ax


def wrap(text, width=22):
    words = text.split()
    lines, cur = [], ""
    for w in words:
        trial = (cur + " " + w).strip()
        if len(trial) > width and cur:
            lines.append(cur)
            cur = w
        else:
            cur = trial
    if cur:
        lines.append(cur)
    return "\n".join(lines)


def text(ax, x, y, s, size=8.2, color=INK, weight="medium", wrap_at=22):
    ax.text(
        x,
        y,
        wrap(s, wrap_at) if "\n" not in s else s,
        ha="center",
        va="center",
        fontsize=size,
        color=color,
        fontweight=weight,
        fontfamily="DejaVu Sans",
        linespacing=1.25,
        zorder=5,
    )


def box(ax, x, y, w, h, fill, edge, radius=0.18):
    p = FancyBboxPatch(
        (x - w / 2, y - h / 2),
        w,
        h,
        boxstyle=f"round,pad=0.02,rounding_size={radius}",
        linewidth=1.4,
        edgecolor=edge,
        facecolor=fill,
        zorder=2,
    )
    ax.add_patch(p)
    return x, y


def stadium(ax, x, y, w, h, fill, edge):
    return box(ax, x, y, w, h, fill, edge, radius=h / 2)


def diamond(ax, x, y, w, h, fill, edge):
    pts = [(x, y + h / 2), (x + w / 2, y), (x, y - h / 2), (x - w / 2, y)]
    p = Polygon(pts, closed=True, linewidth=1.4, edgecolor=edge, facecolor=fill, zorder=2)
    ax.add_patch(p)
    return x, y


def cylinder(ax, x, y, w, h, fill, edge):
    body = Rectangle((x - w / 2, y - h / 2 + 0.12), w, h - 0.24, linewidth=1.3, edgecolor=edge, facecolor=fill, zorder=2)
    top = Ellipse((x, y + h / 2 - 0.12), w, 0.28, linewidth=1.3, edgecolor=edge, facecolor=fill, zorder=3)
    bot = Ellipse((x, y - h / 2 + 0.12), w, 0.28, linewidth=1.3, edgecolor=edge, facecolor=fill, zorder=3)
    ax.add_patch(body)
    ax.add_patch(bot)
    ax.add_patch(top)
    return x, y


def process_circle(ax, x, y, r, fill, edge):
    c = Circle((x, y), r, linewidth=1.4, edgecolor=edge, facecolor=fill, zorder=2)
    ax.add_patch(c)
    return x, y


def arrow(ax, x1, y1, x2, y2, label="", side="right", color=INK):
    ax.annotate(
        "",
        xy=(x2, y2),
        xytext=(x1, y1),
        arrowprops=dict(arrowstyle="-|>", color=color, lw=1.25, mutation_scale=11),
        zorder=1,
    )
    if label:
        mx, my = (x1 + x2) / 2, (y1 + y2) / 2
        dx, dy = x2 - x1, y2 - y1
        if abs(dy) >= abs(dx):
            ox, oy = (0.18 if side == "right" else -0.18), 0.0
        else:
            ox, oy = 0.0, (0.16 if side == "right" else -0.16)
        ax.text(
            mx + ox,
            my + oy,
            label,
            ha="center",
            va="center",
            fontsize=7.2,
            color=TEAL if label in ("Yes",) else (RED if label in ("No",) else GREY),
            fontweight="bold",
            fontfamily="DejaVu Sans",
            zorder=6,
            bbox=dict(boxstyle="round,pad=0.12", facecolor="white", edgecolor="none", alpha=0.9),
        )


def title(ax, w, h, s):
    ax.text(w / 2, h - 0.32, s, ha="center", va="center", fontsize=13, fontweight="bold", color=NAVY, fontfamily="DejaVu Sans")


def save(fig, name):
    path = DIR / name
    fig.savefig(path, bbox_inches="tight", facecolor="white", pad_inches=0.18)
    plt.close(fig)
    print("wrote", path.name, path.stat().st_size, "bytes")


def draw_activity():
    w, h = 13.2, 18.6
    fig, ax = new_fig(w, h)
    title(ax, w, h, "Activity: Track my order status (My Orders)")

    # Happy path x=6.6
    cx = 6.6
    left = 2.35
    right = 10.7

    n = {}
    n["start"] = stadium(ax, cx, 17.5, 3.6, 0.7, FILL_START, NAVY)
    text(ax, *n["start"], "Customer wants to\ncheck an order", size=8)

    n["login"] = diamond(ax, cx, 16.15, 2.5, 1.35, FILL_DEC, GOLD)
    text(ax, *n["login"], "Logged in?", size=8, wrap_at=12)
    arrow(ax, cx, 17.15, cx, 16.82)

    n["go_login"] = box(ax, left, 16.15, 2.5, 0.72, FILL_ACT, NAVY)
    text(ax, *n["go_login"], "Go to login", size=8)
    arrow(ax, 5.35, 16.15, 3.6, 16.15, "No", side="right")

    n["auth"] = diamond(ax, left, 14.7, 2.4, 1.3, FILL_DEC, GOLD)
    text(ax, *n["auth"], "Auth OK?", size=8, wrap_at=10)
    arrow(ax, left, 15.79, left, 15.35)

    n["fail"] = stadium(ax, left, 13.35, 2.5, 0.7, FILL_BAD, RED)
    text(ax, *n["fail"], "Stay on login /\nerror", size=7.5)
    arrow(ax, left, 14.05, left, 13.7, "No")

    n["open"] = box(ax, cx, 14.7, 2.9, 0.75, FILL_ACT, NAVY)
    text(ax, *n["open"], "Open My Orders", size=8)
    arrow(ax, cx, 15.47, cx, 15.08, "Yes")
    arrow(ax, 3.55, 14.7, 5.15, 14.7, "Yes", side="right")

    n["load"] = box(ax, cx, 13.45, 3.6, 0.9, FILL_ACT, NAVY)
    text(ax, *n["load"], "System: load orders\nfor this customer id only", size=7.6)
    arrow(ax, cx, 14.32, cx, 13.9)

    n["any"] = diamond(ax, cx, 11.9, 2.5, 1.35, FILL_DEC, GOLD)
    text(ax, *n["any"], "Any orders?", size=8, wrap_at=12)
    arrow(ax, cx, 13.0, cx, 12.57)

    n["empty"] = box(ax, left, 11.9, 2.8, 0.9, FILL_ACT, NAVY)
    text(ax, *n["empty"], "Show: You have\nno orders yet", size=7.5)
    arrow(ax, 5.35, 11.9, 3.75, 11.9, "No")

    n["end_empty"] = stadium(ax, left, 10.4, 2.8, 0.75, FILL_OK, TEAL)
    text(ax, *n["end_empty"], "Done - no support\ncall needed", size=7.3)
    arrow(ax, left, 11.45, left, 10.78)

    n["size"] = diamond(ax, cx, 10.4, 2.7, 1.4, FILL_DEC, GOLD)
    text(ax, *n["size"], "More than\n20 orders?", size=7.6)
    arrow(ax, cx, 11.22, cx, 11.1, "Yes")

    n["page"] = box(ax, 4.55, 8.85, 2.7, 0.8, FILL_ACT, NAVY)
    text(ax, *n["page"], "Show page of 20,\nnewest first", size=7.4)
    arrow(ax, 5.5, 10.05, 4.55, 9.25, "Yes")

    n["full"] = box(ax, 8.65, 8.85, 2.7, 0.8, FILL_ACT, NAVY)
    text(ax, *n["full"], "Show full list,\nnewest first", size=7.4)
    arrow(ax, 7.7, 10.05, 8.65, 9.25, "No", side="left")

    n["read"] = box(ax, cx, 7.45, 3.5, 0.8, FILL_ACT, NAVY)
    text(ax, *n["read"], "Customer reads date,\namount, status", size=7.6)
    arrow(ax, 4.55, 8.45, 5.4, 7.85)
    arrow(ax, 8.65, 8.45, 7.8, 7.85)

    n["openrow"] = diamond(ax, cx, 6.05, 2.5, 1.3, FILL_DEC, GOLD)
    text(ax, *n["openrow"], "Open a row?", size=8, wrap_at=12)
    arrow(ax, cx, 7.05, cx, 6.7)

    n["end_list"] = stadium(ax, left, 6.05, 2.4, 0.7, FILL_OK, TEAL)
    text(ax, *n["end_list"], "Done", size=8)
    arrow(ax, 5.35, 6.05, 3.55, 6.05, "No")

    n["own"] = diamond(ax, cx, 4.55, 2.8, 1.35, FILL_DEC, GOLD)
    text(ax, *n["own"], "Order belongs to\nthis customer?", size=7.3)
    arrow(ax, cx, 5.4, cx, 5.22, "Yes")

    n["deny"] = box(ax, right, 4.55, 3.1, 0.95, FILL_BAD, RED)
    text(ax, *n["deny"], "Deny access - do not\nshow items or amounts", size=7.2)
    arrow(ax, 8.0, 4.55, 9.15, 4.55, "No")

    n["end_deny"] = stadium(ax, right, 3.15, 2.7, 0.7, FILL_BAD, RED)
    text(ax, *n["end_deny"], "Done - security event", size=7.3)
    arrow(ax, right, 4.07, right, 3.5)

    n["detail"] = box(ax, cx, 3.15, 3.5, 0.85, FILL_ACT, NAVY)
    text(ax, *n["detail"], "Show date, items,\namount, status", size=7.5)
    arrow(ax, cx, 3.87, cx, 3.58, "Yes")

    n["clear"] = diamond(ax, cx, 1.75, 2.6, 1.25, FILL_DEC, GOLD)
    text(ax, *n["clear"], "Status now\nclear?", size=7.6)
    arrow(ax, cx, 2.72, cx, 2.38)

    n["ok"] = stadium(ax, left, 1.75, 2.6, 0.7, FILL_OK, TEAL)
    text(ax, *n["ok"], "Done - self-service", size=7.4)
    arrow(ax, 5.3, 1.75, 3.65, 1.75, "Yes")

    n["support"] = box(ax, right, 1.75, 3.0, 0.85, FILL_ACT, NAVY)
    text(ax, *n["support"], "Contact support\nas exception", size=7.4)
    arrow(ax, 7.9, 1.75, 9.2, 1.75, "No")

    n["end_sup"] = stadium(ax, right, 0.45, 2.8, 0.65, FILL_ACT, GREY)
    text(ax, *n["end_sup"], "Done - remaining call", size=7.3)
    arrow(ax, right, 1.32, right, 0.78)

    ax.text(
        w / 2,
        0.12,
        "L2 Track my order status  |  CAP-01/02/03/04  |  STORY-1..4  |  Happy path ends without support",
        ha="center",
        va="center",
        fontsize=7.2,
        color=GREY,
        fontfamily="DejaVu Sans",
    )
    save(fig, "activity_my_orders.png")


def draw_state():
    w, h = 12.4, 7.6
    fig, ax = new_fig(w, h)
    title(ax, w, h, "State: Order lifecycle shown on My Orders")

    # initial
    ax.add_patch(Circle((1.3, 3.9), 0.16, facecolor=INK, edgecolor=INK, zorder=3))
    ax.text(1.3, 3.45, "start", ha="center", fontsize=7.5, color=GREY, fontfamily="DejaVu Sans")

    pending = box(ax, 4.4, 3.9, 3.1, 1.35, FILL_STATE, NAVY, radius=0.45)
    text(ax, 4.4, 4.05, "Pending", size=11, weight="bold")
    text(ax, 4.4, 3.62, "Only non-terminal\nstatus in this slice", size=7.2)

    completed = box(ax, 9.6, 5.55, 3.0, 1.2, FILL_OK, TEAL, radius=0.45)
    text(ax, 9.6, 5.7, "Completed", size=11, weight="bold")
    text(ax, 9.6, 5.32, "Terminal for this BRD", size=7.2)

    cancelled = box(ax, 9.6, 2.25, 3.0, 1.2, FILL_BAD, RED, radius=0.45)
    text(ax, 9.6, 2.4, "Cancelled", size=11, weight="bold")
    text(ax, 9.6, 2.02, "Terminal; detail still\nshows items + amount", size=7.0)

    # end markers
    ax.add_patch(Circle((12.05, 5.55), 0.22, fill=False, edgecolor=INK, lw=1.6, zorder=3))
    ax.add_patch(Circle((12.05, 5.55), 0.10, facecolor=INK, edgecolor=INK, zorder=4))
    ax.add_patch(Circle((12.05, 2.25), 0.22, fill=False, edgecolor=INK, lw=1.6, zorder=3))
    ax.add_patch(Circle((12.05, 2.25), 0.10, facecolor=INK, edgecolor=INK, zorder=4))

    arrow(ax, 1.48, 3.9, 2.82, 3.9)
    ax.text(2.15, 4.35, "Order created after\nsuccessful payment\n(fulfilment - not this page)", ha="center", va="bottom", fontsize=6.8, color=GREY, fontfamily="DejaVu Sans")

    arrow(ax, 5.95, 4.35, 8.08, 5.35)
    ax.text(6.7, 5.15, "Warehouse dispatched\n/ order closed", ha="center", fontsize=7, color=GREY, fontfamily="DejaVu Sans")

    arrow(ax, 5.95, 3.45, 8.08, 2.55)
    ax.text(6.7, 2.55, "Cancelled on another\nchannel (not CR-01\non this page)", ha="center", va="top", fontsize=7, color=GREY, fontfamily="DejaVu Sans")

    arrow(ax, 11.12, 5.55, 11.82, 5.55)
    arrow(ax, 11.12, 2.25, 11.82, 2.25)

    note = (
        "My Orders is read-only. This page does not fire Place / Complete / Cancel.\n"
        "Customer-facing statuses in scope: Pending, Completed, Cancelled.\n"
        "Out of scope on the UI: Shipped, live courier map, cancel-from-this-page (CR-01)."
    )
    ax.text(w / 2, 0.55, note, ha="center", va="center", fontsize=7.6, color=INK, fontfamily="DejaVu Sans", linespacing=1.35)
    save(fig, "state_order_lifecycle.png")


def draw_dfd_l0():
    w, h = 12.8, 7.8
    fig, ax = new_fig(w, h)
    title(ax, w, h, "DFD Level 0: Track my order status")

    box(ax, 1.7, 4.2, 2.4, 1.1, FILL_ENT, GOLD)
    text(ax, 1.7, 4.2, "Customer", size=10, weight="bold")

    box(ax, 1.7, 1.5, 2.6, 1.1, FILL_ENT, GOLD)
    text(ax, 1.7, 1.5, "Support\n(exception only)", size=8)

    process_circle(ax, 6.3, 4.2, 1.35, FILL_ACT, NAVY)
    text(ax, 6.3, 4.35, "0.0", size=9, weight="bold")
    text(ax, 6.3, 3.95, "Track my\norder status", size=8)

    cylinder(ax, 10.7, 6.15, 2.5, 1.15, FILL_STORE, "#6C3483")
    text(ax, 10.7, 6.15, "Customer\naccounts", size=8)

    cylinder(ax, 10.7, 4.2, 2.5, 1.15, FILL_STORE, "#6C3483")
    text(ax, 10.7, 4.2, "Orders", size=9, weight="bold")

    cylinder(ax, 10.7, 2.25, 2.5, 1.15, FILL_STORE, "#6C3483")
    text(ax, 10.7, 2.25, "Order lines", size=8.5)

    arrow(ax, 2.95, 4.45, 4.95, 4.45)
    ax.text(3.95, 4.85, "identity + list/detail request", ha="center", fontsize=6.8, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 4.95, 3.95, 2.95, 3.95)
    ax.text(3.95, 3.52, "list / empty / detail / denied", ha="center", fontsize=6.8, color=GREY, fontfamily="DejaVu Sans")

    arrow(ax, 7.5, 5.05, 9.45, 5.95)
    ax.text(8.7, 5.85, "read by customer id", ha="center", fontsize=6.6, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 7.65, 4.2, 9.4, 4.2)
    ax.text(8.5, 4.48, "read orders by customer id", ha="center", fontsize=6.6, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 7.5, 3.35, 9.45, 2.5)
    ax.text(8.7, 2.7, "read lines by order id", ha="center", fontsize=6.6, color=GREY, fontfamily="DejaVu Sans")

    # dashed-like exception via thinner grey
    ax.annotate(
        "",
        xy=(1.7, 2.08),
        xytext=(1.7, 3.62),
        arrowprops=dict(arrowstyle="-|>", color=GREY, lw=1.1, linestyle="dashed", mutation_scale=10),
    )
    ax.text(2.55, 2.9, "only if still unclear", ha="left", fontsize=6.6, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 3.05, 1.7, 9.4, 3.85)
    ax.text(6.6, 2.35, "order number lookup", ha="center", fontsize=6.6, color=GREY, fontfamily="DejaVu Sans")

    ax.text(
        w / 2,
        0.28,
        "Payment, warehouse and courier stores are out of this DFD (fulfilment writes; this feature reads).",
        ha="center",
        fontsize=7.2,
        color=GREY,
        fontfamily="DejaVu Sans",
    )
    save(fig, "dfd_order_tracking_l0.png")


def draw_dfd_l1():
    w, h = 14.2, 9.4
    fig, ax = new_fig(w, h)
    title(ax, w, h, "DFD Level 1: processes inside 0.0 Track my order status")

    box(ax, 1.55, 4.3, 2.35, 5.6, FILL_ENT, GOLD, radius=0.25)
    text(ax, 1.55, 6.55, "Customer", size=11, weight="bold")

    # process boxes in a column
    procs = [
        ("1.0 Authenticate", 7.0),
        ("2.0 List my orders", 5.15),
        ("3.0 Show order detail", 3.3),
        ("4.0 Empty-state or deny", 1.45),
    ]
    for label, y in procs:
        box(ax, 6.3, y, 3.5, 1.05, FILL_ACT, NAVY)
        text(ax, 6.3, y, label, size=8.6, weight="bold")

    # boundary
    rect = FancyBboxPatch(
        (4.3, 0.7),
        4.0,
        7.0,
        boxstyle="round,pad=0.04,rounding_size=0.15",
        linewidth=1.1,
        linestyle="--",
        edgecolor=NAVY,
        facecolor="none",
        zorder=1,
    )
    ax.add_patch(rect)
    ax.text(6.3, 7.55, "0.0 Track my order status", ha="center", fontsize=8, color=NAVY, fontfamily="DejaVu Sans", fontweight="bold")

    cylinder(ax, 12.2, 7.0, 2.7, 1.2, FILL_STORE, "#6C3483")
    text(ax, 12.2, 7.0, "D1 Customer\naccounts", size=8)

    cylinder(ax, 12.2, 4.55, 2.7, 1.2, FILL_STORE, "#6C3483")
    text(ax, 12.2, 4.55, "D2 Orders", size=9, weight="bold")

    cylinder(ax, 12.2, 2.15, 2.7, 1.2, FILL_STORE, "#6C3483")
    text(ax, 12.2, 2.15, "D3 Order lines", size=8.4)

    # customer -> processes (left edge 2.725, process left 4.55)
    arrow(ax, 2.73, 6.55, 4.52, 7.0)
    ax.text(3.35, 7.05, "credentials /\nsession", ha="center", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 2.73, 5.15, 4.52, 5.15)
    ax.text(3.55, 5.42, "open My Orders", ha="center", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 2.73, 3.3, 4.52, 3.3)
    ax.text(3.5, 3.57, "selected order id", ha="center", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")

    # process outputs land on distinct heights of the customer box
    arrow(ax, 4.52, 4.85, 2.73, 4.55)
    ax.text(3.6, 4.55, "list rows", ha="center", fontsize=6.4, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 4.52, 3.0, 2.73, 2.85)
    ax.text(3.55, 2.68, "detail payload", ha="center", fontsize=6.4, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 4.52, 1.45, 2.73, 1.7)
    ax.text(3.5, 1.25, "empty or denied", ha="center", fontsize=6.4, color=GREY, fontfamily="DejaVu Sans")

    # internal, offset so they do not run through box centres
    arrow(ax, 5.3, 6.47, 5.3, 5.68)
    ax.text(4.55, 6.05, "customer id", ha="right", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 7.95, 6.47, 7.95, 3.83)
    ax.text(8.35, 5.15, "customer id", ha="left", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 5.3, 4.62, 5.3, 1.98)
    ax.text(4.55, 3.2, "zero rows", ha="right", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 7.3, 2.77, 7.3, 1.98)
    ax.text(7.85, 2.35, "not owner /\ninvalid id", ha="left", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")

    # stores
    arrow(ax, 8.1, 7.15, 10.82, 7.15)
    ax.text(9.45, 7.42, "read / verify", ha="center", fontsize=6.5, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 8.1, 5.15, 10.82, 4.7)
    ax.text(9.5, 5.2, "Orders where\nCustomerID = session", ha="center", fontsize=6.4, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 8.1, 3.45, 10.82, 4.2)
    ax.text(9.6, 3.95, "header if owned", ha="center", fontsize=6.4, color=GREY, fontfamily="DejaVu Sans")
    arrow(ax, 8.1, 3.15, 10.82, 2.35)
    ax.text(9.55, 2.55, "lines if owned", ha="center", fontsize=6.4, color=GREY, fontfamily="DejaVu Sans")

    ax.text(
        w / 2,
        0.28,
        "3.0 must confirm ownership before returning order lines. Empty vs deny are different payloads (process 4.0).",
        ha="center",
        fontsize=7.2,
        color=GREY,
        fontfamily="DejaVu Sans",
    )
    save(fig, "dfd_order_tracking.png")


if __name__ == "__main__":
    draw_activity()
    draw_state()
    draw_dfd_l0()
    draw_dfd_l1()

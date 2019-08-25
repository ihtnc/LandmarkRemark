import icons from "@src/icons";

describe("Icons module", () => {
  test("should define SELF", () => {
    expect(icons.SELF).toEqual("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAYAAACM/rhtAAAABmJLR0QA/wD/AP+gvaeTAAACVklEQVRYw+3Yz2sTQRQH8Nls0pgm3aoXEYtBi0Ij2trUi7WSggf14KFelN4UAyopBYOY9qINXls91B5ERC/iQQSxtIoilaKI/4FCeyr2oiJ4UvH5vuPM5je97cxhH3xhmBl4H7Kb2U2ECMuOSnKOcyZVMG63BZfjrHKoLiucYdO4Ic4fgE4eidH0lYTMicGYRmJt0BQuzvkEyEwxQT+WO2sCqEJ+VnsDL3wyNHwo2oDTyQ1ENfKwCWABzW9c3NQSiDUFLJgA5tF84nxrYOmcD8ybAGbRPJtx6fvbRhzm+ntcDcyaALqcjwCMj8bpWxUSY8wp3Ae110j1cH4Csq/bpbGzcSqcicuxwmEtY/IcdDg3ReMhrTOl9hiprZxFjYm6gjZ3ODIYVyFfcLaYAD7Wl/bZrSStv/L8exBjzGV2+5f6UdA4PGMpvT1Cay+9lsfMF4bu2hHRyGNBAq+j6e2riZY4nblJ/5E3GyTwKZq+uZvaELh8P6WBz4MEyi/H+4cbA7FHARdDYAgMgRYBF5oB1197MtVz7x74wIUggXNoOnWp9mX1wkgb5U+31cxhjwLeCRLYx/mFxnhSaMypXEymyVMEe3tFwDWD5rOlCnCoP0pHs5UfUVhTwGlhoMr1wP17XDqw120GLFsB7NoWoZ38hmMtMNXuUEfSsRP4damTHEfIYGwdcGXe81/zV+c9K4BFNC9f/n8WLt3zD2Q5rjsDiyaAA5y/uOdwQHd3+a/2cow5rGEP56AwVCXOb1H5BfdEpfrvt2vCcKU5I3WfUp+aS5vGhWV9/QN4+MUdZUt6awAAAABJRU5ErkJggg==");
  });

  test("should define YELLOW_DOT", () => {
    expect(icons.YELLOW_DOT).toEqual("http://maps.google.com/mapfiles/ms/icons/yellow-dot.png");
  });

  test("should define GREEN_DOT", () => {
    expect(icons.GREEN_DOT).toEqual("http://maps.google.com/mapfiles/ms/icons/green-dot.png");
  });

  test("should define RED_DOT", () => {
    expect(icons.RED_DOT).toEqual("http://maps.google.com/mapfiles/ms/icons/red-dot.png");
  });

  test("should define BLUE_DOT", () => {
    expect(icons.BLUE_DOT).toEqual("http://maps.google.com/mapfiles/ms/icons/blue-dot.png");
  });

  test("should define FILLED_CHECK_MARK", () => {
    expect(icons.FILLED_CHECK_MARK).toEqual("https://img.icons8.com/cotton/64/000000/checkmark.png");
  });

  test("should define CHECK_MARK", () => {
    expect(icons.CHECK_MARK).toEqual("data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHg9IjBweCIgeT0iMHB4Igp3aWR0aD0iNjQiIGhlaWdodD0iNjQiCnZpZXdCb3g9IjAgMCAxNzIgMTcyIgpzdHlsZT0iIGZpbGw6IzAwMDAwMDsiPjxnIGZpbGw9Im5vbmUiIGZpbGwtcnVsZT0ibm9uemVybyIgc3Ryb2tlPSJub25lIiBzdHJva2Utd2lkdGg9Im5vbmUiIHN0cm9rZS1saW5lY2FwPSJub25lIiBzdHJva2UtbGluZWpvaW49Im1pdGVyIiBzdHJva2UtbWl0ZXJsaW1pdD0iMTAiIHN0cm9rZS1kYXNoYXJyYXk9IiIgc3Ryb2tlLWRhc2hvZmZzZXQ9IjAiIGZvbnQtZmFtaWx5PSJub25lIiBmb250LXdlaWdodD0ibm9uZSIgZm9udC1zaXplPSJub25lIiB0ZXh0LWFuY2hvcj0ibm9uZSIgc3R5bGU9Im1peC1ibGVuZC1tb2RlOiBub3JtYWwiPjxwYXRoIGQ9Ik0wLDE3MnYtMTcyaDE3MnYxNzJ6IiBmaWxsPSJub25lIiBzdHJva2U9Im5vbmUiIHN0cm9rZS13aWR0aD0iMSIgc3Ryb2tlLWxpbmVjYXA9ImJ1dHQiPjwvcGF0aD48Zz48cGF0aCBkPSJNODYsMjEuNWMtMzUuNjIyMzcsMCAtNjQuNSwyOC44Nzc2MyAtNjQuNSw2NC41YzAsMzUuNjIyMzcgMjguODc3NjMsNjQuNSA2NC41LDY0LjVjMzUuNjIyMzcsMCA2NC41LC0yOC44Nzc2MyA2NC41LC02NC41YzAsLTM1LjYyMjM3IC0yOC44Nzc2MywtNjQuNSAtNjQuNSwtNjQuNXoiIGZpbGw9IiMyZWNjNzEiIHN0cm9rZT0ibm9uZSIgc3Ryb2tlLXdpZHRoPSIxIiBzdHJva2UtbGluZWNhcD0iYnV0dCI+PC9wYXRoPjxwYXRoIGQ9Ik04NiwzMy41OTM3NWMtMjguOTQzMTcsMCAtNTIuNDA2MjUsMjMuNDYzMDggLTUyLjQwNjI1LDUyLjQwNjI1YzAsMjguOTQzMTcgMjMuNDYzMDgsNTIuNDA2MjUgNTIuNDA2MjUsNTIuNDA2MjVjMjguOTQzMTcsMCA1Mi40MDYyNSwtMjMuNDYzMDggNTIuNDA2MjUsLTUyLjQwNjI1YzAsLTI4Ljk0MzE3IC0yMy40NjMwOCwtNTIuNDA2MjUgLTUyLjQwNjI1LC01Mi40MDYyNXoiIGZpbGw9IiNmZmZmZmYiIHN0cm9rZT0ibm9uZSIgc3Ryb2tlLXdpZHRoPSIxIiBzdHJva2UtbGluZWNhcD0iYnV0dCI+PC9wYXRoPjxwYXRoIGQ9Ik04NiwyMS41Yy0zNS42MjIzNywwIC02NC41LDI4Ljg3NzYzIC02NC41LDY0LjVjMCwzNS42MjIzNyAyOC44Nzc2Myw2NC41IDY0LjUsNjQuNWMzNS42MjIzNywwIDY0LjUsLTI4Ljg3NzYzIDY0LjUsLTY0LjVjMCwtMzUuNjIyMzcgLTI4Ljg3NzYzLC02NC41IC02NC41LC02NC41eiIgZmlsbD0ibm9uZSIgc3Ryb2tlPSIjZmZmZmZmIiBzdHJva2Utd2lkdGg9IjguMDYyNSIgc3Ryb2tlLWxpbmVjYXA9ImJ1dHQiPjwvcGF0aD48cGF0aCBkPSJNNTYuNDM3NSw5Mi43MTg3NWwxOC4yMDc4MSwxNi4xMjVsNDAuOTE3MTksLTQ3LjAzMTI1IiBmaWxsPSJub25lIiBzdHJva2U9IiMyZWNjNzEiIHN0cm9rZS13aWR0aD0iOC4wNjI1IiBzdHJva2UtbGluZWNhcD0icm91bmQiPjwvcGF0aD48L2c+PC9nPjwvc3ZnPg==");
  });

  test("should define CROSS_MARK", () => {
    expect(icons.CROSS_MARK).toEqual("https://img.icons8.com/cotton/64/000000/delete-sign--v2.png");
  });

  test("should define EXPAND", () => {
    expect(icons.EXPAND).toEqual("https://img.icons8.com/flat_round/64/000000/expand-arrow--v2.png");
  });

  test("should define COLLAPSE", () => {
    expect(icons.COLLAPSE).toEqual("https://img.icons8.com/flat_round/64/000000/collapse-arrow--v2.png");
  });

  test("should define LEFT", () => {
    expect(icons.LEFT).toEqual("https://img.icons8.com/material-sharp/24/000000/double-left.png");
  });

  test("should define RIGHT", () => {
    expect(icons.RIGHT).toEqual("https://img.icons8.com/material-sharp/24/000000/double-right.png");
  });
});
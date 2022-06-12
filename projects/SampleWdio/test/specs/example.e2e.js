const buttonA = '~ButtonA';
const buttonB = '~ButtonB';
const logText = '~LogText';
const cube1 = '//GameObject[@name="Cube (2)"]/Transform';

describe('Click ButtonA', () => {
  beforeEach(async () => {
    await $(logText).clearValue();
  });
  it('appends expected text to LogText', async () => {
    await $(buttonA).click();
    await expect($(logText)).toHaveTextContaining('Button A was clicked');
  });
});

describe('Click ButtonB', () => {
  beforeEach(async () => {
    await $(logText).clearValue();
  });
  it('appends expected text to LogText', async () => {
    await $(buttonB).click();
    await expect($(logText)).toHaveTextContaining('Button B was clicked');
  });
});

describe('SetValue LogText', () => {
  it('text is updated with the value', async () => {
    const value = 'Time: ' + Math.floor(Date.now() / 1000);
    await $(logText).addValue(value);
    await expect($(logText)).toHaveTextContaining(value);
  });
});

describe('Get localPosition', () => {
  it('returns the current local pos', async () => {
    const expectedValue = { x: -0.23, y: 0.4, z: -2.47 };
    const res = await $(cube1).getAttribute('localPosition');
    const actualLocalPos = JSON.parse(res);
    expect(actualLocalPos).toEqual(expectedValue);
  });
});

describe('Get localRotation', () => {
  it('returns the current local rot', async () => {
    const expectedValue = { x: 34.38, y: 304.12, z: 352.29 };
    const res = await $(cube1).getAttribute('localRotation');
    const actualLocalPos = JSON.parse(res);
    expect(actualLocalPos).toEqual(expectedValue);
  });
});

describe('Get localScale', () => {
  it('returns the current local scale', async () => {
    const expectedValue = { x: 0.03, y: 0.03, z: 0.03 };
    const res = await $(cube1).getAttribute('localScale');
    const actualLocalPos = JSON.parse(res);
    expect(actualLocalPos).toEqual(expectedValue);
  });
});

const buttonA = '~ButtonA';
const buttonB = '~ButtonB';
const logText = '~LogText';

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
    const value2 = JSON.stringify(value);
    await $(logText).setValue(value);
    await expect($(logText)).toHaveTextContaining(value);
  });
});

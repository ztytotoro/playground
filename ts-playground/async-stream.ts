class Signal<T> {
  private resolve: (val?: T | PromiseLike<T>) => void = () => {};
  private promise: Promise<T | PromiseLike<T> | void> | null = null;
  #closed = false;

  constructor() {
    this.genPromise();
  }

  private genPromise() {
    this.promise = new Promise<T | void>(resolve => {
      this.resolve = resolve;
    })
  }

  push(val?: T | PromiseLike<T>) {
    this.resolve(val);
    this.genPromise();
  }

  private async getNextVal() {
    return await this.promise;
  }

  async *[Symbol.asyncIterator]() {
    while(!this.#closed) {
      yield await this.getNextVal();
    }
  }

  dispose() {
    this.#closed = true;
    this.promise = null;
    this.resolve = () => {}
  }
}

async function random() {
  await new Promise<void>(resolve => setTimeout(() => resolve(), 1000));
  return Math.random();
}

const sg = new Signal();

const subscribeData = async () => {
  for await(const val of sg) {
    console.log(val);
  }
}

subscribeData().then(() => console.log('done'));

const token = setInterval(() => sg.push(random()), 1000)

setTimeout(() => {
  sg.dispose();
  clearInterval(token)
}, 10000);
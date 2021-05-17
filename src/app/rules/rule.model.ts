export class Rule {
    title: string;
    content: string;
    icon: string;

    public constructor(init?: Partial<Rule>) {
        Object.assign(this, init);
      }
}
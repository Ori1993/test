using Castle.DynamicProxy;
using System.Reflection;

namespace Dynamicproxylearning
{
    public class GenerDecorator : DispatchProxy, IInterceptor
    {
        public object TargetClass { get; set; }

        public void Intercept(IInvocation invocation)
        {
            //事前
            invocation.Proceed();
            //事后
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            //访问我们的服务方法
            var serviceResult = targetMethod.Invoke(TargetClass, args);
            return "";
        }
    }
}
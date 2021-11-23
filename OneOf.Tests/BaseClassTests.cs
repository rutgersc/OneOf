using NUnit.Framework;

namespace OneOf.Tests {
    public class Response : OneOfBase<
        Response.MethodNotAllowed,
        Response.InvokeSuccessResponse,
        Response.AnotherResponse
        >
    {
        Response(OneOf<MethodNotAllowed, InvokeSuccessResponse, AnotherResponse> _) : base(_) { }
        public class MethodNotAllowed {}
        public class InvokeSuccessResponse {}
        public class AnotherResponse { }

        public static implicit operator Response(MethodNotAllowed _) => new Response(_);
        public static implicit operator Response(InvokeSuccessResponse _) => new Response(_);
        public static implicit operator Response(AnotherResponse _) => new Response(_);
    }

    public class BaseClassTests
    {
        [Test]
        public void CanMatchOnBase()
        {
            Response x = new Response.MethodNotAllowed();
            Assert.AreEqual(true, x.Match(
                methodNotAllowed => true,
                invokeSuccessResponse => false,
                anotherResponse => false));
        }

        [Test]
        public void TryPick_Supports_Nullable_Analysis()
        {
            Response response = new Response.MethodNotAllowed();

            if (response.TryPickT0(out var v0, out var remainder))
            {
                // No warning
                _ = v0.ToString();

                try
                {
                    // CS8602 dereference of a possibly null reference
                    _ = remainder.Value;
                }
                catch { }
            }
            else
            {
                if (remainder.Value.TryPickT0(out var v1, out var v2))
                {
                    // No warning
                    _ = v1.ToString();

                    try
                    {
                        // CS8602 dereference of a possibly null reference
                        _ = v2.ToString();
                    }
                    catch { }
                }
            }
        }
    }
}
